using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using bfws.Models;
using Microsoft.AspNetCore.Hosting;
using bfws.Services;
using Microsoft.AspNetCore.Identity;
using bfws.Helpers;

namespace bfws.Views
{
    public class GendersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITransactionLogger _transactionLogger;

        public GendersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _transactionLogger = transactionLogger;
        }

        // GET: Genders
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var genders = await _context.Gender.AsNoTracking().ToListAsync();
            var Paginator = new Paginator<Gender>();
            ViewBag.PaginatorData = Paginator.GetPageData(genders, page, size);

            IEnumerable<Gender> paginatedItems = Paginator.Paginate(genders, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                genders = paginatedItems.ToList();
            }

            if (searchBy == "Name")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    genders = genders.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(genders.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(genders.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        genders = paginatedItems.ToList();
                    }
                }
            }
            else if (searchBy == "Status")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    genders = genders.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(genders.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(genders.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        genders = paginatedItems.ToList();
                    }
                }
            }

            return View(genders);
        }

        // GET: Genders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // GET: Genders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Gender gender)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Add(gender);
                        await _context.SaveChangesAsync();

                        //Log transaction 
                        await _transactionLogger.LogTransaction(_context,await _userManager.GetUserAsync(HttpContext.User),"GenderAdded",gender);
                        //Save Transaction
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gender);
        }

        // GET: Genders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender.SingleOrDefaultAsync(m => m.Id == id);
            if (gender == null)
            {
                return NotFound();
            }
            return View(gender);
        }

        // POST: Genders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name")] Gender gender)
        {
            if (id != gender.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(gender);
                        await _context.SaveChangesAsync();

                        //Log transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User),"GenderEdited",gender);

                        //Save Transaction
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!GenderExists(gender.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gender);
        }

        // GET: Genders/Activate/5
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // POST: Genders/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var gender = await _context.Gender.SingleOrDefaultAsync(m => m.Id == id);
                    gender.Status = "Active";
                    _context.Gender.Update(gender);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "GenderActivated", gender);

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Genders/DeActivate/5
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gender = await _context.Gender
                .SingleOrDefaultAsync(m => m.Id == id);
            if (gender == null)
            {
                return NotFound();
            }

            return View(gender);
        }

        // POST: Genders/DeActivate/5
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                { 
                    var gender = await _context.Gender.SingleOrDefaultAsync(m => m.Id == id);
                    gender.Status = "InActive";
                    _context.Gender.Update(gender);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "GenderDeactivated", gender);

                    // Commit the transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool GenderExists(int id)
        {
            return _context.Gender.Any(e => e.Id == id);
        }
    }
}
