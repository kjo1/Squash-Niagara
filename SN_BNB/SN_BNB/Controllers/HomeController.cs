using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SN_BNB.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SN_BNB.Data;


namespace SN_BNB.Controllers
{
    public class HomeController : Controller
    {
        SNContext _context;

        public HomeController(SNContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    int teamID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).TeamID;

                    var fixture = _context.Fixtures.Include(f => f.AwayTeam)
                                                    .Include(f => f.HomeTeam)
                                                    .Where(f => (f.idHomeTeam == teamID || f.idAwayTeam == teamID)
                                                    && DateTime.Today.AddDays(30).CompareTo(f.FixtureDateTime) > 0).OrderBy(f => f.FixtureDateTime);
                    ViewBag.fixture = fixture;
                }
                catch
                { ViewBag.fixture = new List<Fixture>(); }

                try
                {
                    int teamID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).TeamID;

                    var fixtureLast = _context.Fixtures.Include(f => f.AwayTeam)
                                                    .Include(f => f.HomeTeam)
                                                    .Where(f => (f.idHomeTeam == teamID || f.idAwayTeam == teamID)
                                                    && DateTime.Now.AddDays(-14).CompareTo(f.FixtureDateTime) > 0).OrderBy(f => f.FixtureDateTime);

                    ViewBag.fixtureLast = fixtureLast;
                }
                catch
                { ViewBag.fixtureLast = new List<Fixture>(); }

                try
                {
                    int teamID = _context.Players.FirstOrDefault(p => p.Email == User.Identity.Name).TeamID;

                    var match = _context.Matches.Include(m => m.Player)
                                                    .Include(m => m.AssignedMatchPlayers)
                                                    .Where(m => (m.FixtureID == teamID)
                                                    && DateTime.Now.AddDays(30).CompareTo(m.MatchTime) > 0).OrderBy(m => m.MatchTime);

                    ViewBag.match = match;
                }
                catch
                { ViewBag.fixture = new List<Fixture>(); }
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LeagueRules()
        {
            return View();
        }

        public IActionResult LeagueConstitution()
        {
            return View();
        }

        public IActionResult Documents()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        public IActionResult Schedule()
        {
            return View();
        }

        public IActionResult Standings()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
