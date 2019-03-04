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
        public async Task<IActionResult> Index()
        {
            var sNContext = _context.Fixtures.Include(f => f.Location).Include(f => f.Season);
            return View(await sNContext.ToListAsync());
        }

        // GET: Fixtures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixtures
                .Include(f => f.Location)
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
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address");
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "ID");
            return View();
        }

        // POST: Fixtures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Datetime,HomeScore,AwayScore,HomeTeamID,AwayTeamID,SeasonID,LocationID")] Fixture fixture)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fixture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", fixture.LocationID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "ID", fixture.SeasonID);
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
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", fixture.LocationID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "ID", fixture.SeasonID);
            return View(fixture);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Datetime,HomeScore,AwayScore,HomeTeamID,AwayTeamID,SeasonID,LocationID")] Fixture fixture)
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
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "Address", fixture.LocationID);
            ViewData["SeasonID"] = new SelectList(_context.Seasons, "ID", "ID", fixture.SeasonID);
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
                .Include(f => f.Location)
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


    }
}
