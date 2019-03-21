using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SN_BNB.Data;
using SN_BNB.Models;

namespace SN_BNB.Controllers
{
    public class PlayersController : Controller
    {
        private readonly SNContext _context;
        private int _captainTeamID = int.MinValue;
        private int CaptainTeamID
        {
            get
            {
                if(_captainTeamID == int.MinValue)
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
        public async Task<IActionResult> ExcelUpload([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID")] Player player, IFormFile file)
        {
            //get all of the Teams so we can search through them
            var teams = from t in _context.Teams select t;

            //create a struct to hold player data
            List<PlayerStruct> dataStructs = new List<PlayerStruct>();

            //receive excel file
            ExcelPackage excelPackage;
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
                throw (ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Players
        public async Task<IActionResult> Index(string sortDirection, string sortField, string actionButton, string searchString, int? TeamID)
        {
            PopulateFilterList();

            var players = from p in _context.Players
                            .Include(p => p.Team)
                            .Include(p => p.AssignedMatchPlayers)
                            .ThenInclude(p => p.Match)
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

            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

            if (sortField == "Team")//Sorting by Team
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                         .OrderBy(p => p.Team);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Team);
                }
            }
            else if (sortField == "Player")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.FullName);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.FullName);
                }
            }
            else if (sortField == "Gender")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Gender);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Gender);
                }
            }
            else if (sortField == "Email")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Email);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Email);
                }
            }
            else if (sortField == "Phone")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Phone);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Phone);
                }
            }
            else if (sortField == "Position")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Position);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Position);
                }
            }
            else if (sortField == "Played")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Played);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Played);
                }
            }
            else if (sortField == "Won")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Win);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Win);
                }
            }
            else if (sortField == "Lost")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Loss);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Loss);
                }
            }
            else if (sortField == "For")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.For);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.For);
                }
            }
            else if (sortField == "Against")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Against);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Against);
                }
            }
            else //Sorting by Points - the default sort order
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players.OrderByDescending(p => p.Points);
                }
                else
                {
                    players = players.OrderBy(p => p.Points);
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            ViewBag.CaptainTeamID = CaptainTeamID;

            return View(await players.ToListAsync());
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
                .Include(p => p.AssignedMatchPlayers)
                .ThenInclude( n => n.Match)
                .ThenInclude ( f => f.Fixture)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID")] Player player)
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
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if(User.IsInRole("Captain") && player.TeamID != CaptainTeamID)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Played,Win,Loss,For,Against,Points,TeamID")] Player player)
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

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

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
            ViewData["TeamID"] = new SelectList(dQuery, "ID", "TeamName");
        }

        [HttpGet]
        public JsonResult GetTeams(int? id)
        {
            return Json(TeamSelectList(id));
        }
    }
}
