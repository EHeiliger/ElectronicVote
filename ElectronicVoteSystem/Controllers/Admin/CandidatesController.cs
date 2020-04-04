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

namespace ElectronicVoteSystem.Controllers.Admin
{
    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly ElectronicVotingContext _context;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly IMapper _mapper;

        public CandidatesController(ElectronicVotingContext context, IHostingEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _context = context;
            this._mapper = mapper;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            var electronicVotingContext = _context.Candidate.Include(c => c.Citizen).Include(c => c.Party);
            return View(await electronicVotingContext.ToListAsync());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate
                .Include(c => c.Citizen)
                .Include(c => c.Party)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }

            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id");
            ViewData["PartyId"] = new SelectList(_context.Party, "Id", "Name");
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CandidateViewModel model)
        {
            var candiate = new Candidate();
            string UniqueName = null;
            if (ModelState.IsValid)
            {
                if (model.ProfileAvatar != null)
                {
                    var folderPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    UniqueName = Guid.NewGuid().ToString() + "_" + model.ProfileAvatar.FileName;
                    var filePath = Path.Combine(folderPath, UniqueName);

                    if (filePath != null) model.ProfileAvatar.CopyTo(new FileStream(filePath, mode: FileMode.Create));
                }

                candiate = _mapper.Map<Candidate>(model);
                candiate.ProfileAvatar = UniqueName;
                _context.Add(candiate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", model.CitizenId);
            ViewData["PartyId"] = new SelectList(_context.Party, "Id", "Name", model.PartyId);
            return View(model);
        }

        // GET: Candidates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (!CanModify())
            {
                ViewData["_Redirect"] = "index";
                ViewData["_Error"] = "Entidad no puede ser modificada mientras una elección este activa";
                return View("InvalidOperation");
            }

            if (id == null)
            {
                return NotFound();
            }

            var candidate = await _context.Candidate.FindAsync(id);
            if (candidate == null)
            {
                return NotFound();
            }

           

                var partyViewModel = _mapper.Map<CandidateViewModel>(candidate);
            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", candidate.CitizenId);
            ViewData["PartyId"] = new SelectList(_context.Party, "Id", "Name", candidate.PartyId);
            return View(partyViewModel);

            //return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CandidateViewModel model)
        {

            if (id != model.Id)
            {
                return NotFound();
            }
    
            if (ModelState.IsValid)
            {

                try
                {
                    var candidate = await _context.Candidate.AsNoTracking().FirstOrDefaultAsync(d => d.Id == model.Id);
                    if (!_context.Party.Find(model.PartyId).Status && model.Status == true)
                    {
                        ViewData["_Redirect"] = "index";
                        ViewData["_Error"] = "Partido " + _context.Party.Find(model.PartyId).Name + " debe estar activo";
                        return View("InvalidOperation");
                    }
                    string UniqueName = candidate.ProfileAvatar;

                    if (model.ProfileAvatar != null)
                    {
                        var folderPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        UniqueName = Guid.NewGuid().ToString() + "_" + model.ProfileAvatar.FileName;
                        var filePath = Path.Combine(folderPath, UniqueName);
                        var filePathDelete = Path.Combine(folderPath, "l");
                        if (!string.IsNullOrEmpty(candidate.ProfileAvatar))
                        {
                            if (System.IO.File.Exists(filePathDelete))
                            {
                                var fileInfo = new System.IO.FileInfo(filePathDelete);
                                fileInfo.Delete();
                            }
                        }

                        if (filePath != null)

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                model.ProfileAvatar.CopyTo(fileStream);
                            }

                    }

                    candidate = _mapper.Map<Candidate>(model);
                    candidate.ProfileAvatar = UniqueName;

                    _context.Entry(candidate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(model.Id))
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

            ViewData["CitizenId"] = new SelectList(_context.Citizen, "Id", "Id", model.CitizenId);
            ViewData["PartyId"] = new SelectList(_context.Party, "Id", "Name", model.PartyId);
            return View(model);
        }

        // GET: Candidates/Delete/5
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

            var candidate = await _context.Candidate
                .Include(c => c.Citizen)
                .Include(c => c.Party)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candidate == null)
            {
                return NotFound();
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var candidate = await _context.Candidate.FindAsync(id);
            candidate.Status = false;
            _context.Candidate.Update(candidate);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidate.Any(e => e.Id == id);
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

        //public static bool IsPartyUp(int Id)
        //{
        //    using (ElectronicVotingContext db = new ElectronicVotingContext())
        //    {
        //        Party party = db.Party.Find(Id);

        //        return (party.Status ? true : false);
        //    }
        //}
    }
}
