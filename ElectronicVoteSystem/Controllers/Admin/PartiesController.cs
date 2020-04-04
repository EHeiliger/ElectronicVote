using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElectronicVoteSystem.Models;
using ElectronicVoteSystem.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace ElectronicVoteSystem.Controllers.Admin
{
    [Authorize]
    public class PartiesController : Controller
    {
        private readonly ElectronicVotingContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMapper _mapper;
        public PartiesController(ElectronicVotingContext context, IHostingEnvironment hostingEnvironment, IMapper mapper)
        {
            this.hostingEnvironment = hostingEnvironment;
            this._mapper = mapper;
            _context = context;
        }

        // GET: Parties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Party.ToListAsync());
        }

        // GET: Parties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Party
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // GET: Parties/Create
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

        // POST: Parties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PartyViewModel model)
        {
            var party = new Party();
            string UniqueName = null;
            if (ModelState.IsValid)
            {
                if (model.Logo != null)
                {
                    var folderPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    UniqueName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
                    var filePath = Path.Combine(folderPath, UniqueName);

                    if (filePath != null) model.Logo.CopyTo(new FileStream(filePath, mode: FileMode.Create));
                }
                party = _mapper.Map<Party>(model);
                party.Logo = UniqueName;
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Parties/Edit/5
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


            var party = await _context.Party.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            var partyViewModel = _mapper.Map<PartyViewModel>(party);
            return View(partyViewModel);
        }

        // POST: Parties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PartyViewModel model )
        {
            if (id != model.Id)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                
                
                try

                {
                    //////////////////////////////// bug //////
                    var party = await _context.Party.AsNoTracking().FirstOrDefaultAsync(d => d.Id == model.Id);
                    string UniqueName = party.Logo;
                    if (model.Logo != null )
                    {
                        var folderPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        UniqueName = Guid.NewGuid().ToString() + "_" + model.Logo.FileName;
                        var filePath = Path.Combine(folderPath, UniqueName);
                        var filePathDelete = Path.Combine(folderPath, party.Logo);
                        if (!string.IsNullOrEmpty(party.Logo))
                        {
                            if (System.IO.File.Exists(filePathDelete))
                            {
                                var fileInfo = new System.IO.FileInfo(filePathDelete);
                                
                                fileInfo.Delete();
                            }
                        }

                        if (filePath != null)
                        {
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                model.Logo.CopyTo(fileStream);
                            }

                        }

                    }
                    party = _mapper.Map<Party>(model);
                    party.Logo = UniqueName;
                    _context.Entry(party).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Update(party);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(model.Id))
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
            return View(model);
        }

        // GET: Parties/Delete/5
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

            var party = await _context.Party
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Parties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Party.FindAsync(id);
            party.Status = false;
            _context.Party.Update(party);
            var companeros = _context.Candidate.Where(c => c.PartyId == id);
            foreach (var companero in companeros )
            {
                companero.Status = false;
            }
            _context.Candidate.UpdateRange(companeros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Party.Any(e => e.Id == id);
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
