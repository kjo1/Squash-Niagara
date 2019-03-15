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
    public class FixturesController : Controller
    {
        private readonly SNContext _context;

        public FixturesController(SNContext context)
        {
            _context = context;
        }

        // GET: Fixtures
        public async Task<IActionResult> Index(string searchString, string sortDirection, string sortField, string actionButton)
        {
            var sNContext = _context;
            var fixtures = from f in _context.Fixtures
                           .Include(f=>f.HomeTeam)
                           .Include(f=>f.AwayTeam)
                           .Include(f=>f.Season)
                           .Include(f=>f.Matches)
                           select f;
            if (!String.IsNullOrEmpty(searchString))
            {
                fixtures = fixtures.Where(t => t.HomeTeam.TeamName.ToUpper().Contains(searchString.ToUpper()));
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

            if (sortField == "Time")//Sorting by Date Time
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                         .OrderBy(f => f.FixtureDateTime);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.FixtureDateTime);
                }
            }
            else if (sortField == "Home Score")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.HomeScore);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.HomeScore);
                }
            }
            else if (sortField == "Away Score")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.AwayScore);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.AwayScore);
                }
            }
            else if (sortField == "Home Team")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.HomeTeam.TeamName);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.HomeTeam.TeamName);
                }
            }
            else if (sortField == "Away Team")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.AwayTeam.TeamName);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.AwayTeam.TeamName);
                }
            }
            else if (sortField == "Season")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.Season.Season_Title);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.Season.Season_Title);
                }
            }
            else if (sortField == "City")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.FixtureLocationCity);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.FixtureLocationCity);
                }
            }
            else if (sortField == "Address")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures
                        .OrderBy(f => f.FixtureLocationAddress);
                }
                else
                {
                    fixtures = fixtures
                        .OrderByDescending(f => f.FixtureLocationAddress);
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;


            return View(await fixtures.ToListAsync());
        }

        // GET: Fixtures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.Season)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // GET: Fixtures/Create
        public IActionResult Create()
        {
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title");
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FixtureDateTime,HomeScore,AwayScore,idHomeTeam,idAwayTeam,Season_idSeason,FixtureLocationCity,FixtureLocationAddress")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title", fixture.Season_idSeason);
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View(fixture);
        }

        // GET: Fixtures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures.FindAsync(id);
            if (fixture == null)
            {
                return NotFound();
            }
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title", fixture.Season_idSeason);
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View(fixture);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FixtureDateTime,HomeScore,AwayScore,idHomeTeam,idAwayTeam,Season_idSeason,FixtureLocationCity,FixtureLocationAddress")] Fixture fixture)
        {
            if (id != fixture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixtureExists(fixture.ID))
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
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title", fixture.Season_idSeason);
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View(fixture);
        }

        // GET: Fixtures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.Season)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // POST: Fixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fixture = await _context.Fixtures.FindAsync(id);
            _context.Fixtures.Remove(fixture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FixtureExists(int id)
        {
            return _context.Fixtures.Any(e => e.ID == id);
        }
        //private void PopulateDropDownLists(Fixture fixture = null)
        //{
        //    var dQuery = from d in _context.Teams
        //                 select d;
        //    ViewData["TeamsID"] = new SelectList(dQuery, "ID", "TeamName", fixture?.idHomeTeam);
        //    ViewData["TeamsID"] = new SelectList(dQuery, "ID", "TeamName", fixture?.idAwayTeam);
        //}
    }
}
