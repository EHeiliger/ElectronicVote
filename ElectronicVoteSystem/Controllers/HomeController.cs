using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElectronicVoteSystem.Models;
using ElectronicVoteSystem.Models.Session;
using ElectronicVoteSystem.Models.ViewModels;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Message = ElectronicVoteSystem.Models.Message;

namespace ElectronicVoteSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ElectronicVotingContext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IEmailSender _emailSender;
        public HomeController(ILogger<HomeController> logger, ElectronicVotingContext context, UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
            _logger = logger;
            this._emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            if(User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel","home");
            }
       
            return View();

        }

        public  IActionResult VotingBooth()
        {
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> VotingBooth(voterViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString(Configuration.KeyId, model.Id);
                using (ElectronicVotingContext db = new ElectronicVotingContext())
                {
                    if (!db.Citizen.Where(c => c.Id == model.Id).Any())
                    {
                        ModelState.AddModelError(string.Empty, "Ciudadano no registrado");
                        HttpContext.Session.Clear();
                        return View(model);
                       
                    }

                    Citizen citizen = db.Citizen.Find(model.Id);
                    if (citizen.Status == false)
                    {
                        ModelState.AddModelError(string.Empty, "Ciudadano Inactivo");
                        HttpContext.Session.Clear();
                        return View(model);
                    }

                    return RedirectToAction("Elections", citizen);
                }

            }
            
            return View(model);
        }
      
        public async Task<IActionResult> Elections(Citizen citizen)
        {
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }
            
            using (ElectronicVotingContext db = new ElectronicVotingContext())
            {
                var electiosBallotPapers = db.Election.Where(e => e.DateInit <= DateTime.Now && e.DateEnd >= DateTime.Now && e.Status == true).ToList();
                List<Election> ElectionsToShow = new List<Election>();
                List<Election> InactiveElections = new List<Election>();
                if (electiosBallotPapers.Any())
                {
                    foreach (var election in electiosBallotPapers)
                    {
                        if (!db.Vote.Any(v => v.CitizenId == citizen.Id && v.BallotPaper.ElectionId == election.Id) &&
                            db.Position.Any(p => p.Id == election.PositionId && p.Status == true))
                        {
                            ElectionsToShow.Add(election);
                        }
                        if (db.BallotPaper.Include(b => b.Candidate).Where(c => c.Candidate.Status == true)
                                .Include(b => b.Election).Where(e => e.ElectionId == election.Id).ToList().Count < 2)
                        {
                            InactiveElections.Add(election);
                        }
                    }
                   
                    if (ElectionsToShow.Count > InactiveElections.Count)
                    {
                        return View(ElectionsToShow);
                    }
                    else
                    {
                        string bodymessage = null;
                        foreach (var votes in electiosBallotPapers)
                        {
                            var ballotpapers = _context.BallotPaper.Where(b => b.ElectionId == votes.Id).ToList();
                            foreach (var balloP in ballotpapers)
                            {
                                if (_context.Vote.Any(v => v.BallotPaperId == balloP.Id && v.CitizenId == citizen.Id))
                                {
                                    Citizen candidato = _context.Candidate.Where(c => c.Id == balloP.CandidateId).Select(c => c.Citizen)
                                        .FirstOrDefault();
                                    bodymessage = bodymessage + "\n" +votes.Name + ":" + candidato.Name + " " + candidato.LastName;
                                }

                            }

                        }
                        
                        var messages = new Message(new string[] { "heiliger.eliet@hotmail.com" }, "Notificacion de voto", bodymessage);
                        await _emailSender.SendEmailAsync(messages);
                        HttpContext.Session.Clear();
                        ViewData["_Redirect"] = "VotingBooth";
                        ViewData["_Error"] = "Proceso de Voto completado";
                        return View("InvalidOperation");
                    }
                }
                else
                {
                    HttpContext.Session.Clear();
                    ViewData["_Redirect"] = "VotingBooth";
                    ViewData["_Error"] = "No tiene Eleciones Pendientes";
                    return View("InvalidOperation");
                }
            }
        }

        public async Task<IActionResult> BallotPapers(int Id)
        {
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }
            var ballotPapers = _context.BallotPaper.Include(b => b.Candidate).Where(c => c.Candidate.Status == true).Include(b => b.Election).Where(e => e.ElectionId == Id);
            ViewData["_NuloId"] = _context.BallotPaper.FirstOrDefault(b => b.CandidateId == 16).Id;
            return View( await ballotPapers.ToListAsync());
          
        }

        public async Task<IActionResult> vote(int Id)
        {
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }

            string cedula = HttpContext.Session.GetString(Configuration.KeyId);
            if (ValidateUser(cedula))
            {
                Vote vote = new Vote();
                vote.CitizenId = cedula;
                vote.Date = DateTime.Now;
                vote.BallotPaperId = Id;

                using (ElectronicVotingContext db = new ElectronicVotingContext())
                {
                    Citizen citizen = db.Citizen.Find(HttpContext.Session.GetString(Configuration.KeyId));
                    db.Vote.Add(vote);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Elections", citizen);
                }
            }

            HttpContext.Session.Clear();
            ViewData["_Redirect"] = "VotingBooth";
            ViewData["_Error"] = "No tiene Eleciones Pendientes";
            return View("InvalidOperation");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.Name == "Admin")
            {
                return RedirectToAction("Panel", "home");
            }

            HttpContext.Session.Clear();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

                if (result.Succeeded)
                {
                    HttpContext.Session.SetString(Configuration.KeyId, "admin");
                    return RedirectToAction("Panel", "Home");
                }

                ModelState.AddModelError("", "Upss..");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult OnGetChartData(int id)
        {


            var ballotpapper = _context.BallotPaper.Where(e => e.ElectionId == id).ToList();
            List<votes> votesCount = new List<votes>();
            foreach (var candidate in ballotpapper)
            {
                Citizen citezen = _context.Candidate.Where(c => c.Id == candidate.CandidateId).Select(c => c.Citizen)
                    .FirstOrDefault();
                string name = citezen.Name + " " + citezen.LastName;
                int votesc = _context.Vote.Count(c => c.BallotPaperId == candidate.Id);
                votesCount.Add(new votes(name,votesc));
            }
            var json = votesCount.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Topping"), x => x.CandidateName)
                .NewColumn(new Column(ColumnType.Number, "Slices"), x => x.VoteCount)
                .Build()
                .GetJson();

            return Content(json);
        }
        [Authorize]
        public IActionResult Results(int id)
        {
            Election election = _context.Election.Find(id);

            return View(election);
        }
        public IActionResult Panel()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> ElectionsResultsIndex()
        {
            var electios = _context.Election.Select(e=>e);
            return View(await electios.ToListAsync());
        }


        public static bool ValidateUser(string Id)
        {
            if (Id!=null)
            {
                using (ElectronicVotingContext db = new ElectronicVotingContext())
                {
                    var electiosBallotPapers = db.Election
                        .Where(e => e.DateInit <= DateTime.Now && e.DateEnd >= DateTime.Now).ToList();
                    List<Election> valideElections = new List<Election>();
                    if (electiosBallotPapers.Any())
                    {
                        foreach (var elec in electiosBallotPapers)
                        {
                            if (!db.Vote.Where(v => v.CitizenId == Id && v.BallotPaper.ElectionId == elec.Id).Any())

                            {
                                valideElections.Add(elec);
                            }
                        }

                        if (valideElections.Any())
                        {
                            return (true);
                        }
                        else
                        {
                            return (false);
                        }
                    }

                }
            }
            return (false);
        }

    }
    
}
