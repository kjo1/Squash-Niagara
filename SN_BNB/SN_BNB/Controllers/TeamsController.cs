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

namespace SN_BNB.Controllers
{
    
    public class TeamsController : Controller
    {
        private readonly SNContext _context;
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
        //private readonly ApplicationDbContext _applicationDbContext;

        //public TeamsController(SNContext context, ApplicationDbContext applicationDbContext)
        //{
        //    _context = context;
        //    _applicationDbContext = applicationDbContext;
        //}

        ///* Create a new role with name being Team.TeamName so that we can assign Users to a specific Team.
        //This is done mainly so that we can make sure that Team Captains can only edit their Team. */
        //protected async void MakeTeamRole(Team team)
        //{
        //    //create the role, the title is the Team Name
        //    IdentityRole teamRole = new IdentityRole(team.TeamName);
        //    //check if the role exists
        //    if (!_applicationDbContext.Roles.Contains(teamRole))
        //    {
        //        await _applicationDbContext.Roles.AddAsync(teamRole);
        //    }
        //    //save changes
        //    await _applicationDbContext.SaveChangesAsync();
        //}

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

            ViewBag.CaptainTeamID = CaptainTeamID;
            
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

            //ViewBag.Players = _context.Players.Where(t => t.TeamID == team.ID);     //due to trouble finding a list of players with Team.Players, this is used

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
                /*MakeTeamRole(team); */    //add the Team Name to the User Role list
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
                         orderby d.DivisionName
                         select d;
            ViewData["DivisionID"] = new SelectList(dQuery, "ID", "DivisionName");
        }
        
       
    }
}
