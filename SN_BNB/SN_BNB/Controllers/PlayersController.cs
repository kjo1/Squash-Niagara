using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SN_BNB.Data;
using SN_BNB.Models;
using PagedList.Mvc;

namespace SN_BNB.Controllers
{
    public class PlayersController : Controller
    {
        private readonly SNContext _context;
        public static List<Player> newPlayerList;    //contains all the new players added so they can be shown to the user

        //pls dont delete Kevin! i need this for later im watching you

        private int _divisionUserID = int.MinValue;
        private int DivisionUserID
        {
            get
            {
                if (_divisionUserID == int.MinValue)
                {
                    _divisionUserID = _context.Players.Include(p => p.Team).FirstOrDefault(p => p.Email == User.Identity.Name).Team.DivisionID;
                }
                return _divisionUserID;
            }
        }


        private int _captainTeamID = int.MinValue;
        private int CaptainTeamID
        {
            get
            {
                if (_captainTeamID == int.MinValue)
                {
                    if (User.IsInRole("Captain"))
                        _captainTeamID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).TeamID;
                    else
                        _captainTeamID = -1;
                }
                return _captainTeamID;
            }
        }

        public PlayersController(SNContext context)
        {
            _context = context;
        }

        public struct PlayerStruct
        {
            public string FirstName;
            public string MiddleName;
            public string LastName;
            public string Email;
            public int Phone;
            public string Gender;
            public int Wins;
            public int Losses;
            public int position;
            public Team Team; //need to find using a search
        }

        // GET: Seasons/ExcelUpload
        public IActionResult ExcelUpload()
        {
            return View(new Season());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcelUpload([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID,OrderOfStrength")] Player player, IFormFile file)
        {
            //get all of the Teams so we can search through them
            var teams = from t in _context.Teams select t;

            //create a struct to hold player data
            List<PlayerStruct> dataStructs = new List<PlayerStruct>();

            //receive excel file
            ExcelPackage excelPackage;

            newPlayerList = new List<Player>();    //contains all the new players added so they can be shown to the user
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    excelPackage = new ExcelPackage(memoryStream);
                }
                var worksheet = excelPackage.Workbook.Worksheets[0];

                //parse the file and update struct
                var start = worksheet.Dimension.Start;
                var end = worksheet.Dimension.End;

                for (int row = start.Row; row <= end.Row; row++)
                {
                    PlayerStruct tempStruct = new PlayerStruct
                    {
                        //Get Team object by executing a WHERE with TeamName
                        Team = teams.Where(t => t.TeamName == worksheet.Cells[row, 5].Text).First()
                    };
                    dataStructs.Add(tempStruct);
                }

                //make new players using the struct
                foreach (PlayerStruct playerStruct in dataStructs)
                {
                    Player tempPlayer = new Player();
                    newPlayerList.Add(tempPlayer);
                    _context.Players.Add(tempPlayer);

                    //update tables
                    _context.SaveChanges();
                }

                //update tables
                await _context.SaveChangesAsync();

            }

            //let the user know that the file was not parsed properly
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            //pass the list of new players to the ExcelConfirm() method
            return RedirectToAction("ExcelConfirm");
        }

        // GET: Players/ExcelConfirm
        public IActionResult ExcelConfirm()
        {
            return View(newPlayerList);
        }

        // GET: Players
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string actionButton, string searchString, int? TeamID, int? DivisionID, string postBack, int? page)
        {

			if (User.Identity.IsAuthenticated && String.IsNullOrEmpty(postBack))
			{
				DivisionID = DivisionUserID;
			}

            ViewData["CurrentSort"] = sortOrder;
            ViewData["PlayerSortPar"] = String.IsNullOrEmpty(sortOrder) ? "player_desc" : "";
            ViewData["TeamSortPar"] = sortOrder == "team" ? "team_desc" : "team";
            ViewData["PlayedSortPar"] = sortOrder == "played" ? "played_desc" : "played";
            ViewData["WonSortPar"] = sortOrder == "won" ? "won_desc" : "won";
            ViewData["LostSortPar"] = sortOrder == "lost" ? "lost_desc" : "lost";
            ViewData["ForSortPar"] = sortOrder == "for" ? "for_desc" : "for";
            ViewData["AgainstSortPar"] = sortOrder == "against" ? "against_desc" : "against";
            ViewData["PointsSortPar"] = sortOrder == "points" ? "points_desc" : "points";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            PopulateFilterList();
            var players = from p in _context.Players
                            .Include(p => p.Team)
                            .Include(p => p.HomeMatches)
                            .Include(p => p.AwayMatches)
                          select p;

			if (!String.IsNullOrEmpty(searchString))
			{
				players = players.Where(p => p.LastName.ToUpper().Contains(searchString.ToUpper())
									   || p.FirstName.ToUpper().Contains(searchString.ToUpper()));
			}
            if (TeamID.HasValue)
            {
                players = players.Where(t => t.TeamID == TeamID);
            }
            if (DivisionID.HasValue)
            {
                players = players.Where(t => t.Team.DivisionID == DivisionID);
            }


            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == currentFilter) //Reverse order on same field
                    {
                        sortOrder = String.IsNullOrEmpty(currentFilter) ? "desc" : "";
                    }
                    currentFilter = actionButton;//Sort by the button clicked
                }
            }

            switch (sortOrder)
            {
                case "player_desc":
                    players = players.OrderByDescending(p => p.FullName);
                    break;
                case "team":
                    players = players.OrderBy(p => p.Team.TeamName);
                    break;
                case "team_desc":
                    players = players.OrderByDescending(p => p.Team.TeamName);
                    break;
                case "played":
                    players = players.OrderBy(p => p.Played);
                    break;
                case "played_desc":
                    players = players.OrderByDescending(p => p.Played);
                    break;
                case "won":
                    players = players.OrderBy(p => p.Win);
                    break;
                case "won_desc":
                    players = players.OrderByDescending(p => p.Win);
                    break;
                case "lost":
                    players = players.OrderBy(p => p.Loss);
                    break;
                case "lost_desc":
                    players = players.OrderByDescending(p => p.Loss);
                    break;
                case "for":
                    players = players.OrderBy(p => p.For);
                    break;
                case "for_desc":
                    players = players.OrderByDescending(p => p.For);
                    break;
                case "against":
                    players = players.OrderBy(p => p.Against);
                    break;
                case "against_desc":
                    players = players.OrderByDescending(p => p.Against);
                    break;
                case "points":
                    players = players.OrderBy(p => p.Points);
                    break;
                case "points_desc":
                    players = players.OrderByDescending(p => p.Points);
                    break;
                default:
                    players = players.OrderBy(p => p.FullName);
                    break;
            }

            ViewBag.CaptainTeamID = CaptainTeamID;
			int pageSize = 10;
			return View(await PaginatedList<Player>.CreateAsync(players.AsNoTracking(), page ?? 1, pageSize));
		}

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .Include(p => p.HomeMatches)
                .Include(p => p.AwayMatches)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }


        // GET: Players/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID,OrderOfStrength")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateDropDownLists();
            return View(player);
        }


        // GET: Players/Edit/5
        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (User.IsInRole("Captain") && player.TeamID != CaptainTeamID)
            {
                return RedirectToAction(nameof(Index));
            }

            if (player == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(player);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID,OrderOfStrength")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", player.TeamID);
            return View(player);
        }

        [Authorize(Roles = "Admin")]
        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .Include(p => p.HomeMatches)
                .Include(p => p.AwayMatches)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [Authorize(Roles = "Admin")]
        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }

        private SelectList TeamSelectList(int? id)
        {
            IOrderedQueryable<Team> dQuery;
            if (User.IsInRole("Captain"))
            {
                dQuery = from t in _context.Teams.Where(t => t.ID == CaptainTeamID)
                         orderby t.TeamName
                         select t;
            }
            else
            {
                dQuery = from t in _context.Teams
                         orderby t.TeamName
                         select t;
            }
            return new SelectList(dQuery, "ID", "TeamName", id);
        }

        private void PopulateDropDownLists(Player player = null)
        {
            ViewData["TeamID"] = TeamSelectList(player?.TeamID);
        }

        private void PopulateFilterList()
        {
            var dQuery = from t in _context.Teams
                         orderby t.TeamName
                         select t;
            var dQueryD = from d in _context.Divisions
                          orderby d.DivisionName
                          select d;

            if(User.Identity.IsAuthenticated)
            {
                ViewData["DivisionID"] = new SelectList(dQueryD, "ID", "DivisionName", DivisionUserID);
            }
            else
            {
                ViewData["DivisionID"] = new SelectList(dQueryD, "ID", "DivisionName");
            }
            

            ViewData["TeamID"] = new SelectList(dQuery, "ID", "TeamName");
        }



        [HttpGet]
        public JsonResult GetTeams(int? id)
        {
            return Json(TeamSelectList(id));
        }
    }
}
