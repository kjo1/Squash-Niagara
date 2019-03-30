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

        // GET: Teams
        public async Task<IActionResult> Index(string searchString, int? divisionID, string sortDirection, string sortField, string actionButton, string postBack, int? page)
        {
			if (User.Identity.IsAuthenticated && String.IsNullOrEmpty(postBack))
			{
				page = 1;
				divisionID = DivisionUserID;
			}

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
                    if (actionButton == sortField) //Reverse order on same field
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;//Sort by the button clicked
                }
            }

			if (sortField == "Division")
			{
				if (String.IsNullOrEmpty(sortDirection))
				{
					teams = teams
						.OrderBy(t => t.DivisionID);
				}
				else
				{
					teams = teams
					.OrderByDescending(t => t.DivisionID);
				}
			}

			if (sortField == "Point(s)")
			{
				if (String.IsNullOrEmpty(sortDirection))
					teams = teams.OrderBy(t => t.TeamPoints);
				else
					teams = teams.OrderByDescending(t => t.TeamPoints);
			}
			else if (sortField == "Team Name")
			{
				if (String.IsNullOrEmpty(sortDirection))
					teams = teams.OrderBy(t => t.TeamName);
				else
					teams = teams.OrderByDescending(t => t.TeamName);
			}
			else if (sortField == "Won")
			{
				if (String.IsNullOrEmpty(sortDirection))
					teams = teams.OrderBy(t => t.TeamWins);
				else
					teams = teams.OrderByDescending(t => t.TeamWins);
			}
			else if (sortField == "Lost")
			{
				if (String.IsNullOrEmpty(sortDirection))
					teams = teams.OrderBy(t => t.TeamLosses);
				else
					teams = teams.OrderByDescending(t => t.TeamLosses);
			}

			ViewData["sortField"] = sortField;
			ViewData["sortDirection"] = sortDirection;
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
                .FirstOrDefaultAsync(m => m.ID == id);

            if (team == null)
            {
                return NotFound();
            }

            ViewBag.Fixtures = team.AwayFixtures.Union(team.HomeFixtures).Where(f => DateTime.Now.AddDays(30).CompareTo(f.FixtureDateTime) > 0).OrderBy(f => f.FixtureDateTime).Select(t => new TeamScheduleVM(t));
            ViewBag.Players = _context.Players.Where(t => t.TeamID == team.ID);     //due to trouble finding a list of players with Team.Players, this is used

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
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,TeamName,TeamPoints,TeamCreatedOn,DivisionID")] Team team)
        {
            if (id != team.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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
