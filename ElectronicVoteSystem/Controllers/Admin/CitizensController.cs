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
    public class CitizensController : Controller
    {
        private readonly ElectronicVotingContext _context;

        public CitizensController(ElectronicVotingContext context)
        {
            _context = context;
        }

        // GET: Citizens
        public async Task<IActionResult> Index()
        {
            return View(await _context.Citizen.ToListAsync());
        }

        // GET: Citizens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citizen = await _context.Citizen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (citizen == null)
            {
                return NotFound();
            }

            return View(citizen);
        }

        // GET: Citizens/Create
        public IActionResult Create()
        {
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }

            return View();
        }

        // POST: Citizens/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,LastName,Email,Status")] Citizen citizen)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citizen);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(citizen);
        }

        // GET: Citizens/Edit/5
        public async Task<IActionResult> Edit(string id)
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

            var citizen = await _context.Citizen.FindAsync(id);
            if (citizen == null)
            {
                return NotFound();
            }
            return View(citizen);
        }

        // POST: Citizens/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,LastName,Email,Status")] Citizen citizen)
        {
            if (id != citizen.Id)
            {
                return NotFound();
            }
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }


            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citizen);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitizenExists(citizen.Id))
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
            return View(citizen);
        }

        // GET: Citizens/Delete/5
        public async Task<IActionResult> Delete(string id)
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


            var citizen = await _context.Citizen
                .FirstOrDefaultAsync(m => m.Id == id);
            if (citizen == null)
            {
                return NotFound();
            }

            return View(citizen);
        }

        // POST: Citizens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var citizen = await _context.Citizen.FindAsync(id);
            citizen.Status = false;
            _context.Citizen.Update(citizen);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitizenExists(string id)
        {
            return _context.Citizen.Any(e => e.Id == id);
        }

        public static bool CanModify()
        {
            using (ElectronicVotingContext db = new ElectronicVotingContext())
            {
                if (db.Election.Where(e => e.Status == true).Any())
                {
                    return (false);
                }

                return (true);
            }
        }
    }
}
