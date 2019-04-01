using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;
using SN_BNB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using SN_BNB.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.IO;

namespace SN_BNB.Controllers
{

    public class TeamsController : Controller
    {
        private readonly SNContext _context;

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

        public TeamsController(SNContext context)
        {
            _context = context;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadProfilePicture([Bind("ID,TeamName,TeamPoints,TeamCreatedOn,DivisionID")] Team team, IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string s = Convert.ToBase64String(fileBytes);
                    team.ProfilePicture = s;
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
            }
            return View(team);
        }

        // GET: Teams
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? divisionID, string actionButton, string postBack, int? page)
        {
			if (User.Identity.IsAuthenticated && String.IsNullOrEmpty(postBack))
			{
				page = 1;
				divisionID = DivisionUserID;
			}

            ViewData["CurrentSort"] = sortOrder;
            ViewData["TeamSortPar"] = String.IsNullOrEmpty(sortOrder) ? "team_desc" : "";
            ViewData["WonSortPar"] = sortOrder == "won" ? "won_desc" : "won";
            ViewData["LostSortPar"] = sortOrder == "lost" ? "lost_desc" : "lost";
            ViewData["PointsSortPar"] = sortOrder == "points" ? "points_desc" : "points";
            ViewData["DivisionSortPar"] = sortOrder == "division" ? "division_desc" : "division";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            PopulateDropDownLists();
            var teams = from t in _context.Teams
                        .Include(t => t.Division)
                        .Include(t => t.HomeFixtures)
                        .ThenInclude(t => t.Matches)
                        .Include(t => t.AwayFixtures)
                        .ThenInclude(t => t.Matches)
                        select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                teams = teams.Where(t => t.TeamName.ToUpper().Contains(searchString.ToUpper()));
            }
            if (divisionID.HasValue)
            {
                teams = teams.Where(t => t.DivisionID == divisionID);
            }

            if (!String.IsNullOrEmpty(actionButton)) //Form Submitted so lets sort!
            {
                if (actionButton != "Filter")//Change of sort is requested
                {
                    if (actionButton == currentFilter) //Reverse order on same field
                    {
                        sortOrder = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
                    }
                    currentFilter = actionButton;//Sort by the button clicked
                }
            }

            switch (sortOrder)
            {
                case "team_desc":
                    teams = teams.OrderByDescending(t => t.TeamName);
                    break;
                case "division":
                    teams = teams.OrderBy(t => t.Division);
                    break;
                case "division_desc":
                    teams = teams.OrderByDescending(t => t.Division);
                    break;
                case "won":
                    teams = teams.OrderBy(t => t.TeamWins);
                    break;
                case "won_desc":
                    teams = teams.OrderByDescending(t => t.TeamWins);
                    break;
                case "lost":
                    teams = teams.OrderBy(t => t.TeamLosses);
                    break;
                case "lost_desc":
                    teams = teams.OrderByDescending(t => t.TeamLosses);
                    break;
                case "points":
                    teams = teams.OrderBy(t => t.TeamPoints);
                    break;
                case "points_desc":
                    teams = teams.OrderByDescending(t => t.TeamPoints);
                    break;
                default:
                    teams = teams.OrderBy(t => t.TeamName);
                    break;
            }

			ViewBag.CaptainTeamID = CaptainTeamID;

			int pageSize = 10;
            return View(await PaginatedList<Team>.CreateAsync(teams.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Division)
                .Include(t => t.HomeFixtures)
                .ThenInclude(t => t.AwayTeam)
                .Include(t => t.AwayFixtures)
                .ThenInclude(t => t.HomeTeam)
                .Include(t=>t.Players)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (team == null)
            {
                return NotFound();
            }

            ViewBag.Fixtures = team.AwayFixtures.Union(team.HomeFixtures).Where(f => DateTime.Now.AddDays(30).CompareTo(f.FixtureDateTime) > 0).OrderBy(f => f.FixtureDateTime).Select(t => new TeamScheduleVM(t));

            return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TeamName,TeamPoints,TeamCreatedOn,DivisionID")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                /*MakeTeamRole(team); */    //add the Team Name to the User Role list
                return RedirectToAction(nameof(Index));
            }
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName", team.DivisionID);
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (User.IsInRole("Captain") && team.ID != CaptainTeamID)
            {
                return RedirectToAction(nameof(Index));
            }

            if (team == null)
            {
                return NotFound();
            }
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName", team.DivisionID);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TeamName,TeamPoints,TeamCreatedOn,DivisionID")] Team team, IFormFile file)
        {
            if (id != team.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            Path.GetExtension(file.FileName);
                            string returnString = "data:image/" + Path.GetExtension(file.FileName) + ";base64,";
                            returnString += Convert.ToBase64String(fileBytes);
                            team.ProfilePicture = returnString;
                        }
                    }
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.ID))
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
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName", team.DivisionID);
            return View(team);
        }

        [Authorize(Roles = "Admin")]
        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .Include(t => t.Division)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.ID == id);
        }

        private void PopulateDropDownLists(Team team = null)
        {
            var dQuery = from d in _context.Divisions
                         orderby d.DivisionName
                         select d;
            if (User.Identity.IsAuthenticated)
            {
                ViewData["DivisionID"] = new SelectList(dQuery, "ID", "DivisionName", DivisionUserID);
            }
            else
            {
                ViewData["DivisionID"] = new SelectList(dQuery, "ID", "DivisionName");
            }
            
        }


    }
}
