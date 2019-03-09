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
    public class PlayersController : Controller
    {
        private readonly SNContext _context;

        public PlayersController(SNContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index(string sortDirection, string sortField, string actionButton, string searchString, int? TeamID)
        {
            //var sNContext = _context.Players.Include(p => p.Team);
            PopulateDropDownLists();

            var players = from p in _context.Players
                            .Include(p => p.Team)
                            .Include(p => p.player_Teams)
                            .ThenInclude(p => p.Match)
                            select p;


            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || p.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }
            if (TeamID.HasValue)
            {
                players = players.Where(t => t.TeamID == TeamID);
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

            if (sortField == "Team")//Sorting by Date Time
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                         .OrderBy(p => p.Team);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Team);
                    //.ThenByDescending(t => t.TeamName);
                }
            }
            else if (sortField == "Player")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.FullName);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.FullName);
                }
            }
            else if (sortField == "Gender")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Gender);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Gender);
                }
            }
            else if (sortField == "Email")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Email);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Email);
                }
            }
            else if (sortField == "Phone")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Phone);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Phone);
                }
            }
            else if (sortField == "Position")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Position);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Position);
                }
            }
            else if (sortField == "Win(s)")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Win);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Win);
                }
            }
            else if (sortField == "Lose(es)")
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players
                        .OrderBy(p => p.Loss);
                }
                else
                {
                    players = players
                        .OrderByDescending(p => p.Loss);
                }
            }
            else //Sorting by Win - the default sort order
            {
                if (String.IsNullOrEmpty(sortDirection))
                {
                    players = players.OrderBy(p => p.Win);
                }
                else
                {
                    players = players.OrderByDescending(p => p.Win);
                }
            }

            //Set sort for next time
            ViewData["sortField"] = sortField;
            ViewData["sortDirection"] = sortDirection;


            return View(await players.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Win,Loss,TeamID")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", player.TeamID);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", player.TeamID);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,Win,Loss,TeamID")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.ID))
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
            ViewData["TeamID"] = new SelectList(_context.Teams, "ID", "TeamName", player.TeamID);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }

        private SelectList TeamSelectList(int? id)
        {
            var dQuery = from t in _context.Teams
                         orderby t.TeamName
                         select t;
            return new SelectList(dQuery, "ID", "TeamName", id);
        }

        private void PopulateDropDownLists(Player player = null)
        {
            ViewData["TeamID"] = TeamSelectList(player?.TeamID);
        }

        [HttpGet]
        public JsonResult GetTeams(int? id)
        {
            return Json(TeamSelectList(id));
        }
    }
}
