using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using bfws.Helpers;
using bfws.Services;
using bfws.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace bfws.Controllers
{
    public class MaritalStatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public MaritalStatesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: MaritalStates
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var maritalstates = await _context.MaritalState.AsNoTracking().ToListAsync();
            var Paginator = new Paginator<MaritalState>();
            ViewBag.PaginatorData = Paginator.GetPageData(maritalstates, page, size);

            IEnumerable<MaritalState> paginatedItems = Paginator.Paginate(maritalstates, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                maritalstates = paginatedItems.ToList();
            }

            if (searchBy == "Name")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    maritalstates = maritalstates.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(maritalstates.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(maritalstates.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        maritalstates = paginatedItems.ToList();
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
                    maritalstates = maritalstates.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(maritalstates.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(maritalstates.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        maritalstates = paginatedItems.ToList();
                    }
                }
            }

            return View(maritalstates);
        }

        // GET: MaritalStates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalState = await _context.MaritalState
                .SingleOrDefaultAsync(m => m.Id == id);

            if (maritalState == null)
            {
                return NotFound();
            }

            return View(maritalState);
        }

        // GET: MaritalStates/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: MaritalStates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] MaritalState maritalState)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Add(maritalState);
                        await _context.SaveChangesAsync();

                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "MaritalStateAdded", maritalState);

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        // TODO: Handle failure
                        throw e;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(maritalState);
        }

        // GET: MaritalStates/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalState = await _context.MaritalState.SingleOrDefaultAsync(m => m.Id == id);
            if (maritalState == null)
            {
                return NotFound();
            }
            return View(maritalState);
        }

        // POST: MaritalStates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name")] MaritalState maritalState)
        {
            if (id != maritalState.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(maritalState);
                        await _context.SaveChangesAsync();

                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "MaritalStateEdited", maritalState);

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!MaritalStateExists(maritalState.Id))
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
            return View(maritalState);
        }

        // GET: MaritalStates/Activate/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalState = await _context.MaritalState.SingleOrDefaultAsync(m => m.Id == id);
            if (maritalState == null)
            {
                return NotFound();
            }
            return View(maritalState);
        }

        //POST: MaritalStates/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var maritalState = await _context.MaritalState.SingleOrDefaultAsync(x => x.Id == id);
                    maritalState.Status = "Activate";
                    _context.MaritalState.Update(maritalState);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "MaritalStateActivated", maritalState);

                    //Save Transaction
                    transaction.Commit();
                }
                catch(Exception e)
                {
                    throw e;
                }
            
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MaritalStates/DeActivate/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var maritalState = await _context.MaritalState.SingleOrDefaultAsync(m => m.Id == id);
            if (maritalState == null)
            {
                return NotFound();
            }
            return View(maritalState);
        }

        //POST: MaritalStates/DeActivate/5
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var maritalState = await _context.MaritalState.SingleOrDefaultAsync(x => x.Id == id);
                    maritalState.Status = "InActivate";
                    _context.MaritalState.Update(maritalState);
                    await _context.SaveChangesAsync();

                    //Log transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "MaritalStateDeactivated", maritalState);

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


        private bool MaritalStateExists(int id)
        {
            return _context.MaritalState.Any(e => e.Id == id);
        }
    }
}
