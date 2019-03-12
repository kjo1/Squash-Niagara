using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SN_BNB.Data;
using SN_BNB.Models;
using System.Web;

namespace SN_BNB.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly SNContext _context;

        public SeasonsController(SNContext context)
        {
            _context = context;
        }

        public struct FixtureStruct
        {
            public DateTime FixtureDateTime;
            public string LocationCity;
            public string LocationAddress;
            public Team HomeTeam;     //need to find team id for sql statement
            public Team AwayTeam;
        }

        // GET: Seasons/ExcelUpload
        public IActionResult ExcelUpload()
        {
            return View(new Season());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcelUpload(IFormFile file)
        {
            //get all of the Teams so we can search through them
            var teams = from t in _context.Teams select t;

            //create a struct to hold fixture data
            List<FixtureStruct> dataStructs = new List<FixtureStruct>();

            //receive excel file
            ExcelPackage excelPackage;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    excelPackage = new ExcelPackage(memoryStream);
                }
                var worksheet = excelPackage.Workbook.Worksheets[0];

                //parse the file and update struct
                var start = worksheet.Dimension.Start;
                var end = worksheet.Dimension.End;
                for (int row = start.Row; row<=end.Row; row++)
                { 
                    FixtureStruct tempStruct = new FixtureStruct
                    {
                        FixtureDateTime = DateTime.Parse(worksheet.Cells[row, 1].Text),
                        LocationCity = worksheet.Cells[row, 2].Text,
                        LocationAddress = worksheet.Cells[row, 3].Text,
                        //Get Team object by executing a WHERE with TeamName
                        HomeTeam = teams.Where(t => t.TeamName == worksheet.Cells[row, 4].Text).First(),
                        AwayTeam = teams.Where(t => t.TeamName == worksheet.Cells[row, 5].Text).First()
                    };
                    dataStructs.Add(tempStruct);
                }

                //make a new season
                Season newSeason = new Season();
                newSeason.SeasonStart = DateTime.Now;
                newSeason.Season_Title = "New Season";
                await _context.AddAsync(newSeason);
                await _context.SaveChangesAsync();



                //make new fixtures using the struct
                foreach (FixtureStruct fixtureStruct in dataStructs)
                {
                    Fixture tempFixture = new Fixture();
                    tempFixture.FixtureDateTime = Convert.ToDateTime(fixtureStruct.FixtureDateTime);
                    tempFixture.FixtureLocationCity = fixtureStruct.LocationCity;
                    tempFixture.FixtureLocationAddress = fixtureStruct.LocationAddress;
                    tempFixture.HomeScore = 0;
                    tempFixture.AwayScore = 0;
                    tempFixture.idHomeTeam = _context.Teams.Find(fixtureStruct.HomeTeam.ID).ID;
                    tempFixture.idAwayTeam = _context.Teams.Find(fixtureStruct.AwayTeam.ID).ID;
                    tempFixture.Season_idSeason = _context.Seasons.Find(newSeason.ID).ID;
                    _context.Fixtures.Add(tempFixture);

                    //update tables
                    _context.SaveChanges();
                }

                //update tables
                await _context.SaveChangesAsync();

            }

            //let the user know that the file was not parsed properly
            catch (Exception ex)
            {
                throw (ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
                return RedirectToAction(nameof(Index));
        }

        // GET: Seasons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Seasons.ToListAsync());
        }

        // GET: Seasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Seasons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Season_Title,SeasonStart,SeasonEnd")] Season season)
        {
            if (ModelState.IsValid)
            {
                _context.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(season);
        }

        // GET: Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Season_Title,SeasonStart,SeasonEnd")] Season season)
        {
            if (id != season.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(season);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonExists(season.ID))
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
            return View(season);
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var season = await _context.Seasons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var season = await _context.Seasons.FindAsync(id);
            _context.Seasons.Remove(season);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.ID == id);
        }
    }
}
