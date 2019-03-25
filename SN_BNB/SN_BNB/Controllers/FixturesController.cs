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

        private int _TeamID = int.MinValue;
        private int TeamID
        {
            get
            {
                if (_TeamID == int.MinValue)
                {
                    if (User.Identity.IsAuthenticated)
                        _TeamID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).TeamID;
                    else
                        _TeamID = -1;
                }
                return _TeamID;
            }
        }

        public FixturesController(SNContext context)
        {
            _context = context;
        }

        // GET: Fixtures
        public async Task<IActionResult> Index(string searchString, string sortDirection, string sortField, string actionButton)
        {
            var sNContext = _context;
            var fixtures = from f in _context.Fixtures
                           .Include(f => f.HomeTeam)
                           .Include(f => f.AwayTeam)
                           .Include(f => f.Season)
                           .Include(f => f.Matches)
                           .Include(f => f.Location)
                           select f;
            if (!String.IsNullOrEmpty(searchString))
            {
                fixtures = fixtures.Where(t => t.HomeTeam.TeamName.ToUpper().Contains(searchString.ToUpper())
                                        || t.AwayTeam.TeamName.ToUpper().Contains(searchString.ToUpper())
                                        || t.Season.Season_Title.ToUpper().Contains(searchString.ToUpper()));
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

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewBag.TeamID = TeamID;

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
                .Include(f => f.AwayTeam)
                .Include(f => f.HomeTeam)
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
            PopulateDropDownLists();
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FixtureDateTime,HomeScore,AwayScore,idHomeTeam,idAwayTeam,BonusPoint,Season_idSeason")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                try
                {
                    //update Team.TeamPoints
                    Team HomeTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idHomeTeam);
                    Team AwayTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idAwayTeam);
                    HomeTeam.TeamPoints += fixture.HomeScore;
                    AwayTeam.TeamPoints += fixture.AwayScore;
                    _context.Update(HomeTeam);
                    _context.Update(AwayTeam);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
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

            var fixture = await _context.Fixtures
                .Include(f => f.Season)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(f => f.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(fixture);

            return View(fixture);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FixtureDateTime,HomeScore,AwayScore,idHomeTeam,idAwayTeam,Season_idSeason,BonusPoint")] Fixture fixture)
        {
            if (id != fixture.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //try
                //{
                    _context.Update(fixture);
                    await _context.SaveChangesAsync();
                //}
                //catch (DbUpdateConcurrencyException)
                //{
                //    if (!FixtureExists(fixture.ID))
                //    {
                //        return NotFound();
                //    }
                //    else
                //    {
                //        throw;
                //    }
                //}
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
        private void PopulateDropDownLists(Fixture fixture = null)
        {
            var aQuery = from f in _context.Seasons
                         orderby f.SeasonStart
                         select f;
            ViewData["Season_idSeason"] = new SelectList(aQuery, "ID", "Season_Title", fixture?.Season_idSeason);

            var tQuery = from f in _context.Teams
                         orderby f.TeamName
                         select f;
            ViewData["idHomeTeam"] = new SelectList(tQuery, "ID", "TeamName", fixture?.idHomeTeam);
            ViewData["idAwayTeam"] = new SelectList(tQuery, "ID", "TeamName", fixture?.idAwayTeam);
        }

    }
}
