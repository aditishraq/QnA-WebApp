using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QnAWebApp.Data;
using QnAWebApp.Models;

namespace QnAWebApp.Controllers
{
    public class QnasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QnasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Qnas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Qna.ToListAsync());
        }

        // GET: Qnas/ShowSearchForm
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // PoST: Qnas/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            return View("Index", await _context.Qna.Where(j => j.Question.Contains(SearchPhrase)).ToListAsync()); ;
        }

        // GET: Qnas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qna = await _context.Qna
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qna == null)
            {
                return NotFound();
            }

            return View(qna);
        }

        // GET: Qnas/Create

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qnas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] Qna qna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(qna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(qna);
        }

        // GET: Qnas/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qna = await _context.Qna.FindAsync(id);
            if (qna == null)
            {
                return NotFound();
            }
            return View(qna);
        }

        // POST: Qnas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Qna qna)
        {
            if (id != qna.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(qna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QnaExists(qna.Id))
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
            return View(qna);
        }

        // GET: Qnas/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var qna = await _context.Qna
                .FirstOrDefaultAsync(m => m.Id == id);
            if (qna == null)
            {
                return NotFound();
            }

            return View(qna);
        }

        // POST: Qnas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var qna = await _context.Qna.FindAsync(id);
            if (qna != null)
            {
                _context.Qna.Remove(qna);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QnaExists(int id)
        {
            return _context.Qna.Any(e => e.Id == id);
        }
    }
}
