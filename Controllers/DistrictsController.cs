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
    public class DistrictsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITransactionLogger _transactionLogger;

        public DistrictsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _transactionLogger = transactionLogger;
        }

        // GET: Districts
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var districts = await _context.District.AsNoTracking().ToListAsync();
            var Paginator = new Paginator<District>();
            ViewBag.PaginatorData = Paginator.GetPageData(districts, page, size);

            IEnumerable<District> paginatedItems = Paginator.Paginate(districts, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                districts = paginatedItems.ToList();
            }

            if (searchBy == "Name")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    districts = districts.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(districts.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(districts.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        districts = paginatedItems.ToList();
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
                    districts = districts.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(districts.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(districts.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        districts = paginatedItems.ToList();
                    }
                }
            }

            return View(districts);
        }

        // GET: Districts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
                .SingleOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: Districts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Districts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] District district)
        {
            if (ModelState.IsValid)
            {
                using (var transaction  = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Add(district);
                        await _context.SaveChangesAsync();

                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "DistrictAdded", district);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        // GET: Districts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District.SingleOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        // POST: Districts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name")] District district)
        {
            if (id != district.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using(var transaction = _context.Database.BeginTransaction())
                {  
                    try
                    {
                        _context.Update(district);
                        await _context.SaveChangesAsync();

                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "DistrictEdited", district);
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DistrictExists(district.Id))
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
            return View(district);
        }

        // GET: Districts/Activate/5
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
                .SingleOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/Activate/5
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var district = await _context.District.SingleOrDefaultAsync(m => m.Id == id);
                    district.Status = "Active";
                    _context.District.Update(district);
                    await _context.SaveChangesAsync();


                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "DistrictActivated", district);
                    transaction.Commit();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Districts/DeActivate/5
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var district = await _context.District
                .SingleOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Districts/DeActivate/5
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var district = await _context.District.SingleOrDefaultAsync(m => m.Id == id);
                    district.Status = "InActive";
                    _context.District.Update(district);
                    await _context.SaveChangesAsync();


                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "DistrictDeactivated", district);
                    transaction.Commit();
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.District.Any(e => e.Id == id);
        }
    }
}
