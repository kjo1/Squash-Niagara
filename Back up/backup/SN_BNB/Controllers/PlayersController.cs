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
        public async Task<IActionResult> Index(string searchString)
        {
            PopulateDropDownLists();
            PopulateDropDownListsT();
            var players = from p in _context.Players
                            .Include(p => p.Team)                           
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || p.FirstName.ToUpper().Contains(searchString.ToUpper()));
            }
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
        public async Task<IActionResult> Create([Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,TeamID")] Player player)
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
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,MiddleName,LastName,Gender,Email,Phone,Position,TeamID")] Player player)
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

        private SelectList PositionSelectList(int? id)
        {
            var dQuery = from p in _context.Players
                         orderby p.Position
                         select p;
            return new SelectList(dQuery, "ID", "Position", id);
        }

        private void PopulateDropDownLists(Team team = null)
        {
            ViewData["Position"] = PositionSelectList(team?.DivisionID);
        }

        private SelectList TeamSelectList(int? id)
        {
            var dQuery = from t in _context.Teams
                         orderby t.TeamName
                         select t;
            return new SelectList(dQuery, "ID", "TeamName", id);
        }

        private void PopulateDropDownListsT(Player player = null)
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
