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
        public async Task<IActionResult> Index(string sortDirection, string sortField, string actionButton)
        {
            var sNContext = _context.Fixtures.Include(f => f.Location).Include(f => f.Season);

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

            //if (sortField == "Date Time")//Sorting by Date Time
            //{
            //    if (String.IsNullOrEmpty(sortDirection))
            //    {
            //        Fixture = Fixtures
            //            .OrderBy(p => p.LastName)
            //            .ThenBy(p => p.FirstName);
            //    }
            //    else
            //    {
            //        patients = patients
            //            .OrderByDescending(p => p.LastName)
            //            .ThenByDescending(p => p.FirstName);
            //    }
            //}
            //else if (sortField == "Exp. Yr. Visits")
            //{
            //    if (String.IsNullOrEmpty(sortDirection))
            //    {
            //        patients = patients
            //            .OrderBy(p => p.ExpYrVisits);
            //    }
            //    else
            //    {
            //        patients = patients
            //            .OrderByDescending(p => p.ExpYrVisits);
            //    }
            //}
            //else if (sortField == "Doctor")
            //{
            //    if (String.IsNullOrEmpty(sortDirection))
            //    {
            //        patients = patients
            //            .OrderBy(p => p.Doctor.LastName)
            //            .ThenBy(p => p.Doctor.FirstName);
            //    }
            //    else
            //    {
            //        patients = patients
            //            .OrderByDescending(p => p.Doctor.LastName)
            //            .ThenByDescending(p => p.Doctor.FirstName);
            //    }
            //}
            //else //Sorting by DOB - the default sort order
            //{
            //    if (String.IsNullOrEmpty(sortDirection))
            //    {
            //        patients = patients.OrderBy(p => p.DOB);
            //    }
            //    else
            //    {
            //        patients = patients.OrderByDescending(p => p.DOB);
            //    }
            //}
            ////Set sort for next time
            //ViewData["sortField"] = sortField;
            //ViewData["sortDirection"] = sortDirection;

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
