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
    public class NewsController : Controller
    {
        private readonly SNContext _context;

        public NewsController(SNContext context)
        {
            _context = context;

            //for the sidebar, rather than displaying the date, display the time since it was posted
            DateTime Now = DateTime.Now;
            foreach (News news in _context.News)
            {
                if ((Now - news.Date).Days == 0)
                {
                    if ((Now - news.Date).Hours == 0)
                    {
                        if ((Now - news.Date).Minutes == 0)
                        {
                            news.TimeSince = ((Now - news.Date).Seconds.ToString() + " seconds ago");
                        }
                        else { news.TimeSince = ((Now - news.Date).Minutes.ToString() + " minutes ago"); }
                    }
                    else { news.TimeSince = ((Now - news.Date).Hours.ToString() + " hours ago"); }
                }
                else { news.TimeSince = ((Now - news.Date).Days.ToString() + " days ago"); }
            }
            _context.SaveChangesAsync();
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var news = from n in _context.News
                       select n;
            news = news.OrderByDescending(n => n.Date);     //order by most recent
            return View(await news.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.ID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string Title, string Content)
        {
            try
            {
                News news = new News
                {
                    Date = DateTime.Now
                };
                news.Title = Title;
                news.Content = Content;

                if (ModelState.IsValid)
                {
                    _context.Add(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(news);
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(string ID, string Title, string Content)
        {
            try
            {
                News news = _context.Find<News>(Convert.ToInt32(ID));
                news.Title = Title;
                news.Content = Content;

                if (ModelState.IsValid)
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.ID == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.ID == id);
        }
    }
}
