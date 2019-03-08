﻿using System;
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
    public class TeamScoresController : Controller
    {
        private readonly SNContext _context;

        public TeamScoresController(SNContext context)
        {
            _context = context;
        }

        // GET: TeamScores
        public async Task<IActionResult> Index()
        {
            var sNContext = _context.TeamScores.Include(t => t.Fixture).Include(t => t.Team);
            return View(await sNContext.ToListAsync());
        }

        // GET: TeamScores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.TeamScores
                .Include(t => t.Fixture)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teamScore == null)
            {
                return NotFound();
            }

            return View(teamScore);
        }

        // GET: TeamScores/Create
        public IActionResult Create()
        {
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID");
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View();
        }

        // POST: TeamScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FixtureScore,TeamScoreApprovedBy,TeamID,FixtureID")] TeamScore teamScore)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teamScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", teamScore.FixtureID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", teamScore.TeamID);
            return View(teamScore);
        }

        // GET: TeamScores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.TeamScores.FindAsync(id);
            if (teamScore == null)
            {
                return NotFound();
            }
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", teamScore.FixtureID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", teamScore.TeamID);
            return View(teamScore);
        }

        // POST: TeamScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FixtureScore,TeamScoreApprovedBy,TeamID,FixtureID")] TeamScore teamScore)
        {
            if (id != teamScore.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamScoreExists(teamScore.ID))
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
            ViewData["FixtureID"] = new SelectList(_context.Fixtures, "ID", "ID", teamScore.FixtureID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", teamScore.TeamID);
            return View(teamScore);
        }

        // GET: TeamScores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamScore = await _context.TeamScores
                .Include(t => t.Fixture)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teamScore == null)
            {
                return NotFound();
            }

            return View(teamScore);
        }

        // POST: TeamScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamScore = await _context.TeamScores.FindAsync(id);
            _context.TeamScores.Remove(teamScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamScoreExists(int id)
        {
            return _context.TeamScores.Any(e => e.ID == id);
        }
    }
}