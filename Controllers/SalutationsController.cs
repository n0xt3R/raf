using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using bfws.Models;
using Microsoft.AspNetCore.Hosting;
using bfws.Services;
using bfws.Helpers;

namespace bfws.Views
{
    public class SalutationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITransactionLogger _transactionLogger;

        public SalutationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _transactionLogger = transactionLogger;
        }

        // GET: Salutations
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var salutations = await _context.Salutation.OrderBy(x => x.Name).AsNoTracking().ToListAsync();
            var Paginator = new Paginator<Salutation>();
            ViewBag.PaginatorData = Paginator.GetPageData(salutations, page, size);

            IEnumerable<Salutation> paginatedItems = Paginator.Paginate(salutations, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                salutations = paginatedItems.ToList();
            }
            if (searchBy == "Name")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    salutations = salutations.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(salutations.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(salutations.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        salutations = paginatedItems.ToList();
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
                    salutations = salutations.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(salutations.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(salutations.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        salutations = paginatedItems.ToList();
                    }
                }
            }

            return View(salutations);
        }

        // GET: Salutations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salutation = await _context.Salutation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salutation == null)
            {
                return NotFound();
            }

            return View(salutation);
        }

        // GET: Salutations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Salutations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Salutation salutation)
        {
            if (ModelState.IsValid)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {   
                    _context.Add(salutation);
                    await _context.SaveChangesAsync();

                    //Log Transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User),"SalutationCreated", salutation);

                    //Save Transaction
                    transaction.Commit();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(salutation);
        }

        // GET: Salutations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salutation = await _context.Salutation.SingleOrDefaultAsync(m => m.Id == id);
            if (salutation == null)
            {
                return NotFound();
            }
            return View(salutation);
        }

        // POST: Salutations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name")] Salutation salutation)
        {
            if (id != salutation.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salutation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalutationExists(salutation.Id))
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
            return View(salutation);
        }

        // GET: Salutations/Activate/5
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salutation = await _context.Salutation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salutation == null)
            {
                return NotFound();
            }

            return View(salutation);
        }

        // POST: Salutations/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var salutation = await _context.Salutation.SingleOrDefaultAsync(m => m.Id == id);
                    salutation.Status = "Active";
                    _context.Salutation.Update(salutation);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context,await _userManager.GetUserAsync(HttpContext.User),"SalutationActivated",salutation);

                    //Save transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Salutations/DeActivate/5
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salutation = await _context.Salutation
                .SingleOrDefaultAsync(m => m.Id == id);
            if (salutation == null)
            {
                return NotFound();
            }

            return View(salutation);
        }

        // POST: Salutations/DeActivate/5
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var salutation = await _context.Salutation.SingleOrDefaultAsync(m => m.Id == id);
                    salutation.Status = "InActive";
                    _context.Salutation.Update(salutation);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "SalutationDeactivated", salutation);

                    //Save transaction
                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SalutationExists(int id)
        {
            return _context.Salutation.Any(e => e.Id == id);
        }
    }
}
