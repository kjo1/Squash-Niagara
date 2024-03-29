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
    public class MatchesController : Controller
    {
        private readonly SNContext _context;

        public MatchesController(SNContext context)
        {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index(string searchString, int? divisionID, string sortOrder, string currentFilter, string actionButton, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["HomeSortPar"] = String.IsNullOrEmpty(sortOrder) ? "home" : "";
            ViewData["TimeSortPar"] = sortOrder == "time" ? "time_desc" : "time";
            ViewData["FixtureSortPar"] = sortOrder == "fixture" ? "fixture_desc" : "fixture";
            ViewData["AwaySortPar"] = sortOrder == "away" ? "away_desc" : "away";
            ViewData["HomeScoreSortPar"] = sortOrder == "hscore" ? "hscore_desc" : "hscore";
            ViewData["AwayScoreSortPar"] = sortOrder == "ascore" ? "ascore_desc" : "ascore";
            ViewData["PositionSortPar"] = sortOrder == "position" ? "position_desc" : "position";

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
            var sNContext = _context;
            var matches = from m in _context.Matches
                          .Include(m => m.Fixture)
                            .ThenInclude(m => m.HomeTeam)
                           .Include(m => m.Fixture)
                            .ThenInclude(m => m.AwayTeam)
                           .Include(m => m.Fixture)
                            .ThenInclude(m => m.Season)
                          .Include(m => m.Player1)
                          .Include(m => m.Player2)
                          select m;


            if (!string.IsNullOrEmpty(searchString))
            {
                matches = matches.Where(m => m.Player1.FirstName.ToUpper().Contains(searchString.ToUpper())
                                        || m.Player1.LastName.ToUpper().Contains(searchString.ToUpper())
                                        || m.Player2.FirstName.ToUpper().Contains(searchString.ToUpper())
                                        || m.Player2.LastName.ToUpper().Contains(searchString.ToUpper())
                                        || m.Fixture.HomeTeam.TeamName.ToUpper().Contains(searchString.ToUpper())
                                        || m.Fixture.AwayTeam.TeamName.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "time":
                    matches = matches.OrderBy(f => f.MatchTime);
                    break;
                case "time_desc":
                    matches = matches.OrderByDescending(f => f.MatchTime);
                    break;
                case "fixture":
                    matches = matches.OrderBy(f => f.Fixture);
                    break;
                case "fixture_desc":
                    matches = matches.OrderByDescending(f => f.Fixture);
                    break;
                case "away":
                    matches = matches.OrderBy(f => f.Player2);
                    break;
                case "away_desc":
                    matches = matches.OrderByDescending(f => f.Player2);
                    break;
                case "hscore":
                    matches = matches.OrderBy(f => f.Player1Score);
                    break;
                case "hscore_desc":
                    matches = matches.OrderByDescending(f => f.Player1Score);
                    break;
                case "ascore":
                    matches = matches.OrderBy(f => f.Player2Score);
                    break;
                case "ascore_desc":
                    matches = matches.OrderByDescending(f => f.Player2Score);
                    break;
                case "position":
                    matches = matches.OrderBy(f => f.MatchPosition);
                    break;
                case "position_desc":
                    matches = matches.OrderByDescending(f => f.MatchPosition);
                    break;
                default:
                    matches = matches.OrderByDescending(f => f.Player1);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Match>.CreateAsync(matches.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.HomeTeam)
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.AwayTeam)
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.Season)
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            PopulateDropDownLists();
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Player1Score,Player2Score,MatchPosition,MatchTime,FixtureID,Player1ID,Player2ID")] Match match)
        {
            if (ModelState.IsValid)
            {
                /* Update Player objects */
                Player player1 = _context.Players.Where(m => m.ID == match.Player1ID).FirstOrDefault();
                Player player2 = _context.Players.Where(m => m.ID == match.Player2ID).FirstOrDefault();
                if (match.Player1Score > match.Player2Score)
                {
                    player1.Win += 1;
                    player2.Loss += 1;
                }
                else if(match.Player1Score < match.Player2Score)
                {
                    player1.Loss += 1;
                    player2.Win += 1;
                }
                player1.Played += 1;
                player2.Played += 1;
                player1.HomeMatches.Add(match);
                player2.AwayMatches.Add(match);
                _context.Update(player1);
                _context.Update(player2);
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["Player1ID"] = new SelectList(_context.Players, "ID", "Email", match.Player1ID);
            ViewData["Player2ID"] = new SelectList(_context.Players, "ID", "Email", match.Player2ID);
            return View(match);
        }

        // GET: Matches/Edit/5
        [Authorize(Roles = "Admin, Captain")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            PopulateDropDownLists(match);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Captain")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Player1Score,Player2Score,MatchPosition,MatchTime,FixtureID,Player1ID,Player2ID")] Match match)
        {
            if (id != match.ID)
            {
                return NotFound();
            }
             // geting the matches
            //var fixtureID = _context.Matches.Include(m => m.Fixture).Where(f => f.ID == id);
            //var fixID = _context.Fixtures.Include(m => m.Matches).Where(f => f.Matches.First(m => f.ID == id).FixtureID == fixtureID);
            if (ModelState.IsValid)
            {
                try
                {
                    /* Update Player objects */
                    Match oldMatch = _context.Matches.AsNoTracking().Where(m => m.ID == match.ID)
                        .Include(m=>m.Fixture)
                           .ThenInclude(f=>f.Season)
                        .Include(m=>m.Fixture)
                           .ThenInclude(f=>f.HomeTeam)
                        .Include(m=>m.Fixture)
                           .ThenInclude(f=>f.AwayTeam)
                        .FirstOrDefault();
                    Player player1 = _context.Players.Where(m => m.ID == match.Player1ID).FirstOrDefault();
                    Player player2 = _context.Players.Where(m => m.ID == match.Player2ID).FirstOrDefault();
                    int oldWinner = 0;  //0 if no winner, 1 if player1, 2 if player2
                    if (oldMatch.Player1Score > oldMatch.Player2Score)
                    {
                        oldWinner = 1;
                    }
                    else if (oldMatch.Player1Score < oldMatch.Player2Score)
                    {
                        oldWinner = 2;
                    }

                    if (match.Player1Score > match.Player2Score)
                    {
                        if (oldWinner == 0)
                        {
                            player1.Win += 1;
                            player2.Loss += 1;
                        }
                        else if (oldWinner == 2)
                        {
                            player1.Win += 1;
                            player1.Loss -= 1;
                            player2.Win -= 1;
                            player2.Loss += 1;
                        }
                    }
                    else if (match.Player1Score < match.Player2Score)
                    {
                        if (oldWinner == 0)
                        {
                            player1.Win += 1;
                            player2.Loss += 1;
                        }
                        else if (oldWinner == 1)
                        {
                            player1.Win -= 1;
                            player1.Loss += 1;
                            player2.Win += 1;
                            player2.Loss -= 1;
                        }
                    }
                    _context.Update(player1);
                    _context.Update(player2);

                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                string fmatch = _context.Matches.First(m => m.ID == id).FixtureID.ToString();
                return RedirectToAction("Details/" + fmatch, "Fixtures");
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["Player1ID"] = new SelectList(_context.Players, "ID", "Email", match.Player1ID);
            ViewData["Player2ID"] = new SelectList(_context.Players, "ID", "Email", match.Player2ID);
            return View(match);
        }

        // GET: Matches/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.HomeTeam)
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.AwayTeam)
                .Include(m => m.Fixture)
                   .ThenInclude(m => m.Season)
                .Include(m => m.Player1)
                .Include(m => m.Player2)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);

            /* Update Player objects */
            Match oldMatch = _context.Matches.AsNoTracking().Where(m => m.ID == match.ID).FirstOrDefault();
            Player player1 = _context.Players.Where(m => m.ID == match.Player1ID).FirstOrDefault();
            Player player2 = _context.Players.Where(m => m.ID == match.Player2ID).FirstOrDefault();
            if (oldMatch.Player1Score > oldMatch.Player2Score)
            {
                player1.Win -= 1;
                player2.Loss -= 1;
            }
            else if (oldMatch.Player1Score < oldMatch.Player2Score)
            {
                player1.Loss -= 1;
                player2.Win -= 1;
            }
            player1.Played -= 1;
            player2.Played -= 1;
            player1.HomeMatches.Remove(match);
            player2.AwayMatches.Remove(match);
            _context.Update(player1);
            _context.Update(player2);

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.ID == id);
        }
        private void PopulateDropDownLists(Match match = null)
        {
            var aQuery = from m in _context.Fixtures.Include(m =>m.AwayTeam).Include(m=> m.HomeTeam)
                         orderby m.FixtureDateTime
                         select m;
            ViewData["FixtureID"] = new SelectList(aQuery, "ID", "Title");

            var bQuery = from m in _context.Players
                         orderby m.FullName
                         select m;
            ViewData["Player1ID"] = new SelectList(bQuery, "ID", "FullName", match?.Player1ID);
            ViewData["Player2ID"] = new SelectList(bQuery, "ID", "FullName", match?.Player2ID);
        }
    }
}
