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
        public async Task<IActionResult> Index(string sortDirection, string sortField, string actionButton, string searchString, int? DivisionID)
        {
            //var sNContext = _context.Teams.Include(t => t.Division);
            PopulateDropDownLists();
            var teams = from t in _context.Teams
                        .Include(t => t.Division)
                        .Include(t => t.Fixture_has_Teams)
                        .Include(t => t.Players)
                            //.Include(t => t.Teamscore)
                        select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                teams = teams.Where(t => t.TeamName.ToUpper().Contains(searchString));
            }
            if (DivisionID.HasValue)
            {
                teams = teams.Where(t => t.DivisionID == DivisionID);
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

            if (sortField == "Team Name")//Sorting by Date Time
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamName);
                    //.ThenBy(p => p.FirstName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamName);
                    //.ThenByDescending(p => p.FirstName);
                }
            }
            else if (sortField == "Point(s)")

                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamPoints);
                    //.ThenBy(p => p.Doctor.FirstName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamPoints);
                    //.ThenByDescending(p => p.Doctor.FirstName);
                }
            else if (sortField == "Created On")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.TeamCreatedOn);
                    //.ThenBy(p => p.Doctor.FirstName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.TeamCreatedOn);
                    //.ThenByDescending(p => p.Doctor.FirstName);
                }
            }
            else if (sortField == "Division")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams
                        .OrderBy(t => t.Division.DivisionName);
                    //.ThenBy(p => p.Doctor.FirstName);
                }
                else
                {
                    teams = teams
                        .OrderByDescending(t => t.Division.DivisionName);
                    //.ThenByDescending(p => p.Doctor.FirstName);
                }
            }
            else //Sorting by Season - the default sort order
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    teams = teams.OrderBy(t => t.TeamPoints);
                }
                else
                {
                    teams = teams.OrderByDescending(f => f.TeamPoints);
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

        private SelectList DivisionSelectList(int? id)
        {
            var dQuery = from d in _context.Divisions
                         orderby d.DivisionName
                         select d;
            return new SelectList(dQuery, "ID", "DivisionName", id);
        }

        private void PopulateDropDownLists(Team team = null)
        {
            ViewData["DivisionID"] = DivisionSelectList(team?.DivisionID);
        }
        [HttpGet]
        public JsonResult GetDivisions(int? id)
        {
            return Json(DivisionSelectList(id));
        }
    }
}