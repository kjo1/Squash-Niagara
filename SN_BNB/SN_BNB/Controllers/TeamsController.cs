using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;
using SN_BNB.Models;

namespace SN_BNB.Controllers
{
    public class TeamsController : Controller
    {
        private readonly SNContext _context;

        public TeamsController(SNContext context)
        {
            _context = context;
        }

        // GET: Teams
        public async Task<IActionResult> Index(string searchString, int? divisionID, string sortDirection, string sortField, string actionButton)
        {
            PopulateDropDownLists();
            var teams = from t in _context.Teams
                        .Include(t => t.Division)
                        .Include(t => t.Players)
                        .Include(t => t.Season_has_Teams)
                        .ThenInclude( t => t.Season)                     
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

            if (sortField == "Created On")//Sorting by Date Time
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                   teams = teams
                        .OrderBy(t => t.TeamCreatedOn);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamCreatedOn);
                }
            }
            else if (sortField == "Team Name")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamName);
                }
            }
            else if (sortField == "Team Points")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamPoints);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamPoints);
                }
            }
            else if (sortField == "Team Name")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamName);
                }
            }
            else if (sortField == "Division")
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
            else if (sortField == "Win(s)")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamWins);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamWins);
                }
            }
            else if (sortField == "Loss(es)")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamLosses);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamLosses);
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;


            return View(await teams.ToListAsync());

           
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
                .FirstOrDefaultAsync(m => m.ID == id);
            if (team == null)
            {
                return NotFound();
            }

            ViewBag.Players = _context.Players.Where(t => t.TeamID == team.ID);     //due to trouble finding a list of players with Team.Players, this is used

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,TeamName,TeamPoints,TeamCreatedOn,DivisionID")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DivisionID"] = new SelectList(_context.Divisions, "ID", "DivisionName", team.DivisionID);
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
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
                         select d;
            ViewData["DivisionID"] = new SelectList(dQuery, "ID", "DivisionName", team?.DivisionID);
        }
        
       
    }
}
