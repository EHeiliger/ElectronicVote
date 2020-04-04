using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicVoteSystem.Models;

namespace ElectronicVoteSystem.Controllers.borrar
{
    public class VotesController : Controller
    {
        private readonly ElectronicVotingContext _context;

        public VotesController(ElectronicVotingContext context)
        {
            _context = context;
        }

        // GET: Votes
        public async Task<IActionResult> Index()
        {
            var electronicVotingContext = _context.Vote.Include(v => v.BallotPaper).Include(v => v.Citizen);
            return View(await electronicVotingContext.ToListAsync());
        }

        // GET: Votes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Vote
                .Include(v => v.BallotPaper)
                .Include(v => v.Citizen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // GET: Votes/Create
        public IActionResult Create()
        {
            ViewData["BallotPaperId"] = new SelectList(_context.BallotPaper, "Id", "Id");
            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id");
            return View();
        }

        // POST: Votes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BallotPaperId,CitizenId,Date")] Vote vote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BallotPaperId"] = new SelectList(_context.BallotPaper, "Id", "Id", vote.BallotPaperId);
            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", vote.CitizenId);
            return View(vote);
        }

        // GET: Votes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Vote.FindAsync(id);
            if (vote == null)
            {
                return NotFound();
            }
            ViewData["BallotPaperId"] = new SelectList(_context.BallotPaper, "Id", "Id", vote.BallotPaperId);
            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", vote.CitizenId);
            return View(vote);
        }

        // POST: Votes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BallotPaperId,CitizenId,Date")] Vote vote)
        {
            if (id != vote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoteExists(vote.Id))
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
            ViewData["BallotPaperId"] = new SelectList(_context.BallotPaper, "Id", "Id", vote.BallotPaperId);
            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", vote.CitizenId);
            return View(vote);
        }

        // GET: Votes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vote = await _context.Vote
                .Include(v => v.BallotPaper)
                .Include(v => v.Citizen)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vote == null)
            {
                return NotFound();
            }

            return View(vote);
        }

        // POST: Votes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vote = await _context.Vote.FindAsync(id);
            _context.Vote.Remove(vote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoteExists(int id)
        {
            return _context.Vote.Any(e => e.Id == id);
        }
    }
}
