﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
		public async Task<IActionResult> Index(string searchString, string sortDirection, string sortField, string actionButton, bool RadioChecked, int? DivisionID, string postBack, int? page)
        {
            if (User.Identity.IsAuthenticated && String.IsNullOrEmpty(postBack))
            {
                DivisionID = DivisionUserID;
            }

            PopulateDropDownLists();
            var sNContext = _context;
            var fixtures = from f in _context.Fixtures
                           .Include(f => f.HomeTeam)
                           .Include(f => f.AwayTeam)
                           .Include(f => f.Season)
                           .Include(f => f.Matches)
                           .Include(f => f.Location)
                           select f;

            if (DivisionID.HasValue)
            {
                fixtures = fixtures.Where(f => (f.AwayTeam.DivisionID == DivisionID) || (f.HomeTeam.DivisionID == DivisionID));
            }

            if(RadioChecked is true)
            {
                fixtures = fixtures.Where(f => (f.HomeTeam.ID == TeamID) || (f.AwayTeam.ID == TeamID));
            }

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
            else //Sorting by Date - the default sort order
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    fixtures = fixtures.OrderByDescending(f => f.FixtureDateTime);
                }
                else
                {
                    fixtures = fixtures.OrderBy(f => f.FixtureDateTime);
                }
            }
            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;
            ViewBag.TeamID = TeamID;

			int pageSize = 10;
			return View(await PaginatedList<Fixture>.CreateAsync(fixtures.AsNoTracking(), page ?? 1, pageSize));
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
                .Include(f=> f.Matches)
                    .ThenInclude(f => f.Player1)
                .Include(f => f.Matches)
                    .ThenInclude(f => f.Player2)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (fixture == null)
            {
                return NotFound();
            }
            ViewBag.match1 = _context.Matches.Where(m => m.FixtureID == fixture.ID);

            return View(fixture);
        }

        [Authorize(Roles ="Admin")]
        // GET: Fixtures/Create
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FixtureDateTime,HomeScore,AwayScore,idHomeTeam,idAwayTeam,Season_idSeason,BonusPoint")] Fixture fixture)
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
                    if(HomeTeam.DivisionID != AwayTeam.DivisionID)
                    {
                        throw new DbUpdateException("Teams must be in the same division.", new Exception());
                    }
                    HomeTeam.TeamPoints += fixture.HomeScore;
                    AwayTeam.TeamPoints += fixture.AwayScore;
                    _context.Update(HomeTeam);
                    _context.Update(AwayTeam);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException dex) // figure out the key
                {
                    ModelState.AddModelError("idHomeTeam", dex.Message);
                    //return BadRequest(ModelState);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }                               
            }
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title", fixture.Season_idSeason);
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View(fixture);

        }

        [Authorize(Roles = "Admin")]
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
                .Include( f => f.Matches)
                    .ThenInclude(f=> f.Player1)
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
        [Authorize(Roles = "Admin")]
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
                Fixture oldFixture = _context.Fixtures.AsNoTracking().Where(P => P.ID == fixture.ID).FirstOrDefault();     //get the old entry
                await _context.SaveChangesAsync();
                _context.Update(fixture);
                try
                {
                    //update Team.TeamPoints
                    Team HomeTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idHomeTeam);
                    Team AwayTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idAwayTeam);
                    if (HomeTeam.DivisionID != AwayTeam.DivisionID)
                    {
                        throw new DbUpdateException("Teams must be in the same division.", new Exception());
                    }
                    HomeTeam.TeamPoints += (fixture.HomeScore - oldFixture.HomeScore);
                    AwayTeam.TeamPoints += (fixture.AwayScore - oldFixture.AwayScore);
                    _context.Update(HomeTeam);
                    _context.Update(AwayTeam);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dex)
                {
                    ModelState.AddModelError("", dex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Unidentified error, contact administrator. ");
                }
                
            }
            ViewData["Season_idSeason"] = new SelectList(_context.Seasons, "ID", "Season_Title", fixture.Season_idSeason);
            ViewData["idHomeTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            ViewData["idAwayTeam"] = new SelectList(_context.Teams, "ID", "TeamName");
            //ViewBag.Matches = _context.Matches.Where(m => m.FixtureID == fixture.ID);
            return View(fixture);
        }

        // GET: Fixtures/Delete/5
        [Authorize(Roles = "Admin")]
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
            try
            {
                //update Team.TeamPoints
                Team HomeTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idHomeTeam);
                Team AwayTeam = _context.Teams.FirstOrDefault(t => t.ID == fixture.idAwayTeam);
                HomeTeam.TeamPoints -= fixture.HomeScore;
                AwayTeam.TeamPoints -= fixture.AwayScore;
                _context.Update(HomeTeam);
                _context.Update(AwayTeam);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            await _context.SaveChangesAsync();
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

            var dQuery = from f in _context.Divisions
                         orderby f.DivisionName
                         select f;
            ViewData["idHomeTeam"] = new SelectList(tQuery, "ID", "TeamName", fixture?.idHomeTeam);
            ViewData["idAwayTeam"] = new SelectList(tQuery, "ID", "TeamName", fixture?.idAwayTeam);

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
