using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicVoteSystem.Models;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicVoteSystem.Controllers.Admin
{
    [Authorize]
    public class BallotPapersController : Controller
    {
        private readonly ElectronicVotingContext _context;

        public BallotPapersController(ElectronicVotingContext context)
        {
            _context = context;
        }

        // GET: BallotPapers
        public async Task<IActionResult> Index()
        {
            var electronicVotingContext = _context.BallotPaper.Include(b => b.Candidate).Include(b => b.Election);
            return View(await electronicVotingContext.ToListAsync());
        }

        // GET: BallotPapers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ballotPaper = await _context.BallotPaper
                .Include(b => b.Candidate)
                .Include(b => b.Election)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ballotPaper == null)
            {
                return NotFound();
            }

            return View(ballotPaper);
        }

        // GET: BallotPapers/Create
        public IActionResult Create()
        {
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidate, "Id", "CitizenId");
            ViewData["ElectionId"] = new SelectList(_context.Election, "Id", "Name");
            return View();
        }

        // POST: BallotPapers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ElectionId,Status,CandidateId")] BallotPaper ballotPaper)
        {
            if (ModelState.IsValid)
            {
                ballotPaper.Status ??= false;
                _context.Add(ballotPaper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidate, "Id", "CitizenId", ballotPaper.CandidateId);
            ViewData["ElectionId"] = new SelectList(_context.Election, "Id", "Name", ballotPaper.ElectionId);
            return View(ballotPaper);
        }

        // GET: BallotPapers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }

            var ballotPaper = await _context.BallotPaper.FindAsync(id);
            if (ballotPaper == null)
            {
                return NotFound();
            }
            ViewData["CandidateId"] = new SelectList(_context.Candidate, "Id", "CitizenId", ballotPaper.CandidateId);
            ViewData["ElectionId"] = new SelectList(_context.Election, "Id", "Name", ballotPaper.ElectionId);
            return View(ballotPaper);
        }

        // POST: BallotPapers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ElectionId,Status,CandidateId")] BallotPaper ballotPaper)
        {
            if (id != ballotPaper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ballotPaper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BallotPaperExists(ballotPaper.Id))
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
            ViewData["CandidateId"] = new SelectList(_context.Candidate, "Id", "CitizenId", ballotPaper.CandidateId);
            ViewData["ElectionId"] = new SelectList(_context.Election, "Id", "Name", ballotPaper.ElectionId);
            return View(ballotPaper);
        }

        // GET: BallotPapers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }

            var ballotPaper = await _context.BallotPaper
                .Include(b => b.Candidate)
                .Include(b => b.Election)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ballotPaper == null)
            {
                return NotFound();
            }

            return View(ballotPaper);
        }

        // POST: BallotPapers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var ballotPaper = await _context.BallotPaper.FindAsync(id);
            ballotPaper.Status = false;
            _context.BallotPaper.Update(ballotPaper);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BallotPaperExists(int id)
        {
            return _context.BallotPaper.Any(e => e.Id == id);
        }
        public static bool CanModify()
        {
            using (ElectronicVotingContext db = new ElectronicVotingContext())
            {
                if (db.Election.Where(e => e.Status == true).Any())
                {
                    return (false);
                }
                else
                {
                    return (true);
                }

            }
        }
    }
}
