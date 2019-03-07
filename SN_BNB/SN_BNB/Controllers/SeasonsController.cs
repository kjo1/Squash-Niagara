using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;
using SN_BNB.Models;
using SN_BNB.ViewModels;
using OfficeOpenXml;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace SN_BNB.Controllers
{
    public class SeasonsController : Controller
    {
        private readonly SNContext _context;

        public SeasonsController(SNContext context)
        {
            _context = context;
        }

        // GET: Seasons/ExcelUpload
        public IActionResult ExcelUpload()
        {
            return View(new Season());
        }

        [HttpPost]
        public async Task<IActionResult> ExcelUpload(Season season)
        {
            System.Diagnostics.Debug.WriteLine("TESTTESTTESTTESTTESTTESTTESTTESTTESTTEST");

            //create a struct to hold fixture data
            FixtureStruct[] dataStructs = new FixtureStruct[2000];

            //receive excel file
            Byte[] file = season.ExcelFile;
            ExcelPackage excelPackage;
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await memoryStream.WriteAsync(file, 0, file.Length);
                    excelPackage = new ExcelPackage(memoryStream);
                }
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

                //parse the file and update struct
                int row = 1;
                while (true)
                {

                    if (worksheet.Cells[row, 1].Value.ToString() == "") break;
                    FixtureStruct tempStruct = new FixtureStruct
                    {
                        FixtureDateTime = worksheet.Cells[row, 1].Value.ToString(),
                        Location = worksheet.Cells[row, 2].Value.ToString(),
                        HomeTeam = worksheet.Cells[row, 3].Value.ToString(),
                        AwayTeam = worksheet.Cells[row, 4].Value.ToString()
                    };

                    row += 1;
                    dataStructs.Append(tempStruct);
                }
                //find location id, hometeam id, and awayteam id
                //make a new season
                _context.Add(new Season());

                //update fixture table
                _context.SaveChanges();

            }
            //let the user know that the file was not parsed properly
            catch { }

            return RedirectToAction(nameof(Index));
        }

        // GET: Seasons
        public async Task<IActionResult> Index()
        {
            var sNContext = _context.Seasons
                .Include(t => t.Fixtures)
                .Include(t => t.Teams)
                .ThenInclude(st => st.Team);
            return View(await sNContext.ToListAsync());
            //return View(await _context.Seasons.ToListAsync());
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
            var Season = new Season
            {
                Teams = new List<Season_has_Team>()
            };
            PopulateAssignedTeamData(Season);
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SeasonStart,SeasonEnd")] Season season)
        {
            if (ModelState.IsValid)
            {
                _context.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(season);
        }
        public struct FixtureStruct
        {
            public string FixtureDateTime;
            public string Location;     //need to find the location id for sql statement
            public string HomeTeam;     //need to find team id for sql statement
            public string AwayTeam;
        }

        //public async Task<IActionResult> CreateFixtures(IFormFile excelDoc)
        //{
        //    //create a struct to hold fixture data
        //    FixtureStruct[] dataStructs = new FixtureStruct[2000];

        //    //receive excel file
        //    ExcelPackage excelPackage;
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await excelDoc.CopyToAsync(memoryStream);
        //        excelPackage = new ExcelPackage(memoryStream);
        //    }
        //    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[1];

        //    //parse the file and update struct
        //    int row = 1;
        //    while (true)
        //    {

        //        if (worksheet.Cells[row, 1].Value.ToString() == "") break;
        //        FixtureStruct tempStruct = new FixtureStruct();

        //        tempStruct.FixtureDateTime = worksheet.Cells[row, 1].Value.ToString();
        //        tempStruct.Location = worksheet.Cells[row, 2].Value.ToString();
        //        tempStruct.HomeTeam = worksheet.Cells[row, 3].Value.ToString();
        //        tempStruct.AwayTeam = worksheet.Cells[row, 4].Value.ToString();

        //        row += 1;
        //        dataStructs.Append(tempStruct);
        //    }
        //    //find location id, hometeam id, and awayteam id
        //    //make a new season
        //    _context.Add(new Season());
        //    //update fixture table
        //    _context.SaveChanges();

        //    return RedirectToAction(nameof(Index));
        //}

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
        public async Task<IActionResult> Edit(int id, [Bind("ID,SeasonStart,SeasonEnd")] Season season)
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

        private void PopulateAssignedTeamData(Season season)
        {
            var AllTeams = _context.Teams;
            var sTeams = new HashSet<int>(season.Teams.Select(b => b.TeamID));
            var viewModel = new List<AssignedTeamVM>();
            foreach (var team in AllTeams)
            {
                viewModel.Add(new AssignedTeamVM
                {
                    TeamID = team.ID,
                    TeamName = team.TeamName,
                    Assigned = sTeams.Contains(team.ID)
                });
            }
            ViewData["Teams"] = viewModel;
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.ID == id);
        }
    }
}
