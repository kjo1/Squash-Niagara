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
        public static List<Fixture> newFixtureList;    //contains all the new fixtures added so they can be shown to the user

        public SeasonsController(SNContext context)
        {
            _context = context;
        }

        public struct FixtureStruct
        {
            public DateTime FixtureDate;
            public string LocationCity;
            public string LocationAddress;
            public Team HomeTeam;     //need to find team id for sql statement
            public Team AwayTeam;
            public List<MatchStruct> Matches;
        }

        public struct MatchStruct
        {
            public TimeSpan MatchTime;
            public Player Player1;
            public Player Player2;
            public int Position;
        }

        // GET: Seasons/ExcelUpload
        public IActionResult ExcelUpload()
        {
            return View(new Season());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExcelUpload([Bind("ID,Season_Title")] Season season, IFormFile file)
        {
            //get all of the Teams and Players so we can search through them
            var teams = from t in _context.Teams select t;
            var players = from p in _context.Players select p;

            //create a struct to hold fixture data
            List<FixtureStruct> dataStructs = new List<FixtureStruct>();

            //receive excel file
            ExcelPackage excelPackage;

            newFixtureList = new List<Fixture>();    //contains all the new fixtures added so they can be shown to the user

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

                DateTime matchDate;
                for (int row = start.Row; row<=end.Row; row+=5)
                {
                    //convert to a DateTime
                    if(!DateTime.TryParseExact(worksheet.Cells[row, 1].Text, "dd/mm/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out matchDate))
                    {
                        //show the user an error
                    }
                    FixtureStruct fixtureStruct = new FixtureStruct
                    {
                        FixtureDate = matchDate,
                        LocationCity = worksheet.Cells[row, 2].Text,
                        LocationAddress = worksheet.Cells[row, 3].Text,

                        //Get Team object by executing a WHERE with TeamName
                        HomeTeam = teams.FirstOrDefault(t => t.TeamName == worksheet.Cells[row, 4].Text),
                        AwayTeam = teams.FirstOrDefault(t => t.TeamName == worksheet.Cells[row, 5].Text)
                    };
                    //loop through the Matches and update struct
                    fixtureStruct.Matches = new List<MatchStruct>(4);
                    for (int i = 1; i <= 4; i++)
                    {
                        MatchStruct matchStruct = new MatchStruct
                        {
                            Position = Convert.ToInt32(worksheet.Cells[row+i, 2].Text),
                            MatchTime = TimeSpan.ParseExact(worksheet.Cells[row+i, 3].Text, "hh\\:mm", null),

                            //Get Player object by executing a WHERE with TeamName
                            Player1 = players.FirstOrDefault(t => (t.FirstName + " " + t.LastName) == worksheet.Cells[row+i, 4].Text),
                            Player2 = players.FirstOrDefault(t => (t.FirstName + " " + t.LastName) == worksheet.Cells[row+i, 5].Text),
                        };
                        fixtureStruct.Matches.Add(matchStruct);
                    }
                    dataStructs.Add(fixtureStruct);
                }

                //make a new season
                season.SeasonStart = DateTime.Now;

                await _context.AddAsync(season);
                await _context.SaveChangesAsync();

                //make new fixtures using the struct
                foreach (FixtureStruct fixtureStruct in dataStructs)
                {
                    Fixture tempFixture = new Fixture();
                    tempFixture.FixtureDateTime = fixtureStruct.FixtureDate;
                    tempFixture.idHomeTeam = _context.Teams.Find(fixtureStruct.HomeTeam.ID).ID;
                    tempFixture.idAwayTeam = _context.Teams.Find(fixtureStruct.AwayTeam.ID).ID;
                    tempFixture.Season_idSeason = _context.Seasons.Find(season.ID).ID;
                    _context.Fixtures.Add(tempFixture);
                    newFixtureList.Add(tempFixture);

                    //update tables
                    _context.SaveChanges();

                    //make the new matches using the structs in each FixtureStruct
                    foreach(MatchStruct matchStruct in fixtureStruct.Matches)
                    {
                        Match tempMatch = new Match();
                        tempMatch.FixtureID = tempFixture.ID;
                        tempMatch.Player1 = matchStruct.Player1;
                        tempMatch.Player2 = matchStruct.Player2;
                        tempMatch.MatchTime = matchStruct.MatchTime;
                        tempMatch.MatchPosition = matchStruct.Position;
                        await _context.Matches.AddAsync(tempMatch);
                    }
                    //update tables
                    _context.SaveChanges();
                }

                //update tables
                await _context.SaveChangesAsync();
            }

            //let the user know that the file was not parsed properly
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("##############################");
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                System.Diagnostics.Debug.WriteLine("##############################");
            }

            //pass the list of new fixtures to the ExcelConfirm() method
            return RedirectToAction("ExcelConfirm");
        }

        // GET: Seasons/ExcelConfirm
        public IActionResult ExcelConfirm()
        {
            return View(newFixtureList);
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
                .Include(s => s.Fixtures)
                .ThenInclude(f => f.Matches)
                .ThenInclude(m=>m.Player1)
                .Include(s => s.Fixtures)
                .ThenInclude(f => f.Matches)
                .ThenInclude(m => m.Player2)
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

        // GET: Seasons/EditExcel/5
        public async Task<IActionResult> EditExcel(int? id)
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
        // POST: Seasons/EditExcel/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditExcel(int id, [Bind("ID,Season_Title,SeasonStart,SeasonEnd")] Season season)
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
                return Redirect("ExcelConfirm");
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

        public struct PlayerMatchStruct
        {
            public Player MatchPlayer;
            public int PreviousPostion;
            public int CurrentPosition;
        }

        // GET: Seasons/Inconsistencies
        public async Task<IActionResult> Inconsistencies()
        {
            /* Make a list of PlayerMatchStructs */
            List<PlayerMatchStruct> matchStructsList = new List<PlayerMatchStruct>();
            var seasons = await _context.Seasons.ToListAsync();

            /* Loop through each fixture */
            foreach (Fixture fixture in seasons.FirstOrDefault().Fixtures)
            {
                /* Loop through each match in the fixture */
                foreach(Match match in fixture.Matches)
                {
                    /* For each Match, track the Players positions in the PlayerMatchStructs list */
                    if (!match.FlaggedForInconsistencies == null)        //has been checked before, skip
                    {
                        continue;
                    }

                    /* If the difference between a previous position and it's following is greater than 1, set a flag */
                    if (true)
                    {
                        match.FlaggedForInconsistencies = true;
                    }
                    else
                    {
                        match.FlaggedForInconsistencies = false;
                    }
                }
            }
            /* Alert the user to inconsistencies */
            /* Display the page */
            return View(seasons);
        }

        private bool SeasonExists(int id)
        {
            return _context.Seasons.Any(e => e.ID == id);
        }
    }
}
