using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicVoteSystem.Models;
using ElectronicVoteSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ElectronicVoteSystem.Controllers.Admin
{
    [Authorize]
    public class ElectionsController : Controller
    {
        private readonly ElectronicVotingContext _context;
        private readonly IMapper _mapper;

        public ElectionsController(ElectronicVotingContext context, IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
        }

        // GET: Elections
        public async Task<IActionResult> Index()
        {
            var electronicVotingContext = _context.Election.Include(e => e.Position);
            return View(await electronicVotingContext.ToListAsync());
        }

        // GET: Elections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var election = await _context.Election
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // GET: Elections/Create
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_context.Position, "Id", "Description");
            return View();
        }

        // POST: Elections/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( ElectionViewModel model)
        {
        
            
            if (ModelState.IsValid)
            {
                model.DateInit = new DateTime(model.DateInit.Year, model.DateInit.Month, model.DateInit.Day, model.InitTime.Hour, model.InitTime.Minute, 0);
                model.DateEnd = new DateTime(model.DateEnd.Year, model.DateEnd.Month, model.DateEnd.Day, model.EndTime.Hour, model.EndTime.Minute, 0);
                Election election = _mapper.Map<Election>(model);
                _context.Add(election);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Position, "Id", "Description", model.PositionId);
            return View(model);
        }

        public async Task<IActionResult> ActiveElection(int Id)
        {
            Election election = _context.Election.Find(Id);

            election.Status = !election.Status;
            _context.Update(election);
            await _context.SaveChangesAsync();

            return Redirect("/elections/index");
        }


        // GET: Elections/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
       
            var election = await _context.Election.FindAsync(id);

            if (election == null)
            {
                return NotFound();
            }

            ViewData["PositionId"] = new SelectList(_context.Position, "Id", "Name", election.PositionId);
            var electionViewModel = _mapper.Map<ElectionViewModel>(election);
            electionViewModel.InitTime = electionViewModel.DateInit;
            electionViewModel.EndTime = electionViewModel.DateEnd;
            return View(electionViewModel);
        }

        // POST: Elections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ElectionViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    model.DateInit = new DateTime(model.DateInit.Year, model.DateInit.Month, model.DateInit.Day, model.InitTime.Hour, model.InitTime.Minute, 0);
                    model.DateEnd = new DateTime(model.DateEnd.Year, model.DateEnd.Month, model.DateEnd.Day, model.EndTime.Hour, model.EndTime.Minute, 0);
                    Election election = _mapper.Map<Election>(model);
                    _context.Update(election);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElectionExists(model.Id))
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
            ViewData["PositionId"] = new SelectList(_context.Position, "Id", "Name", model.PositionId);
            return View(model);
        }

        // GET: Elections/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var election = await _context.Election
                .Include(e => e.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (election == null)
            {
                return NotFound();
            }

            return View(election);
        }

        // POST: Elections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var election = await _context.Election.FindAsync(id);
            election.Status = false;
            _context.Election.Update(election);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElectionExists(int id)
        {
            return _context.Election.Any(e => e.Id == id);
        }
    }
}
