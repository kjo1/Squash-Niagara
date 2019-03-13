using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SN_BNB.Data;
using SN_BNB.Models;
using SN_BNB.ViewModels;

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
        public async Task<IActionResult> Index(string SearchPlayer, string sortDirection, string sortField, string actionButton)
        {
            var matches = from d in _context.Matches
                          .Include(m => m.AssignedMatchPlayers).ThenInclude(d => d.Player)
                          select d;
            //var sNContext = _context.Matches.Include(m => m.Fixture).Include(m => m.Player);
            if (!string.IsNullOrEmpty(SearchPlayer))
            {
                matches = matches.Where(m => m.Player.FullName.Contains(SearchPlayer));
            }

            if (!String.IsNullOrEmpty(actionButton))
            {
                if (actionButton != "Filter")
                {
                    if (actionButton == sortField)
                    {
                        sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                    }
                    sortField = actionButton;
                }
            }
            if(sortField == "Players")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    matches = matches
                        .OrderBy(m => m.Player);
                }
                else
                {
                    matches = matches
                        .OrderByDescending(m => m.Player);
                }
            }
            else if (sortField == "Position")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    matches = matches
                        .OrderBy(m => m.MatchPosition);
                }
                else
                {
                    matches = matches
                        .OrderByDescending(m => m.MatchPosition);
                }
            }
            else if (sortField == "Date & Time")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    matches = matches
                        .OrderBy(m => m.MatchDateTime);
                }
                else
                {
                    matches = matches
                        .OrderByDescending(m => m.MatchDateTime);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    matches = matches
                        .OrderBy(m => m.Player);
                }
                else
                {
                    matches = matches
                        .OrderByDescending(m => m.Player);
                }
            }
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;

            return View(await matches.ToListAsync());
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
                .Include(m => m.Player)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "Title");
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "Email");

            Match match = new Match();
            PopulateAssignedPlayerData(match);
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Player1Score,Player2Score,MatchPosition,MatchDateTime,FixtureID,PlayerID")] Match match, string[] selectedOptions)
        {
            try
            {
                UpdateMatchPlayers(selectedOptions, match);
                if (ModelState.IsValid)
                {
                    _context.Add(match);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
                ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "Email", match.PlayerID);

            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Unable to save changes");
            }
            PopulateAssignedPlayerData(match);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AssignedMatchPlayers).ThenInclude(m => m.Player)
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.ID == id);

            if (match == null)
            {
                return NotFound();
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "Email", match.PlayerID);
            PopulateAssignedPlayerData(match);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Player1Score,Player2Score,MatchPosition,MatchDateTime,FixtureID,PlayerID")] Match match, string[] selectedOptions)
        {
            var matchToUpdate = await _context.Matches
                .Include(m => m.AssignedMatchPlayers).ThenInclude(d => d.Player)
                .SingleOrDefaultAsync(d => d.ID == id);

            if (id != match.ID)
            {
                return NotFound();
            }

            UpdateMatchPlayers(selectedOptions, matchToUpdate);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes");
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", match.FixtureID);
            ViewData["PlayerID"] = new SelectList(_context.Players, "ID", "Email", match.PlayerID);
            PopulateAssignedPlayerData(matchToUpdate);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.Fixture)
                .Include(m => m.Player)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateAssignedPlayerData(Match match)
        {
            var allPlayers = _context.Players;
            var matPlayers = new HashSet<int>(match.AssignedMatchPlayers.Select(b => b.PlayerID));
            var selected = new List<PlayerMatchVM>();
            var available = new List<PlayerMatchVM>();
            foreach (var p in allPlayers)
            {
                if (matPlayers.Contains(p.ID))
                {
                    selected.Add(new PlayerMatchVM
                    {
                        PlayerID = p.ID,
                        Name = p.FullName
                    });
                }
                else
                {
                    available.Add(new PlayerMatchVM
                    {
                        PlayerID = p.ID,
                        Name = p.FullName
                    });
                }
            }
            ViewData["selOpts"] = new MultiSelectList(selected.OrderBy(s => s.Name), "PlayerID", "Name");
            ViewData["availOpts"] = new MultiSelectList(available.OrderBy(s => s.Name), "PlayerID", "Name");
        }

        private void UpdateMatchPlayers(string[] selectedOptions, Match matchToUpdate)
        {
            if(selectedOptions == null)
            {
                matchToUpdate.AssignedMatchPlayers = new List<AssignedMatchPlayer>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var matplayers = new HashSet<int>(matchToUpdate.AssignedMatchPlayers.Select(b => b.MatchID));
            foreach (var p in _context.Players)
            {
                if (selectedOptionsHS.Contains(p.ID.ToString()))
                {
                    if (!matplayers.Contains(p.ID))
                    {
                        matchToUpdate.AssignedMatchPlayers.Add(new AssignedMatchPlayer
                        {
                            PlayerID = p.ID,
                            MatchID = matchToUpdate.ID
                        });
                    }
                }
                else
                {
                    if (matplayers.Contains(p.ID))
                    {
                        AssignedMatchPlayer playerToRemove = matchToUpdate.AssignedMatchPlayers.SingleOrDefault<AssignedMatchPlayer>();
                            _context.Remove(playerToRemove);
                    }
                }
            }
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.ID == id);
        }
    }
}
