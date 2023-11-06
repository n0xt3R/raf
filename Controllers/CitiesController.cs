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
using Microsoft.AspNetCore.Authorization;
using bfws.Services;
using bfws.Models;
using Microsoft.AspNetCore.Identity;


namespace bfws.Controllers
{
    public class CitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CitiesController(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: Cities
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var cities = await _context.City.AsNoTracking().ToListAsync();
            var Paginator = new Paginator<City>();
            ViewBag.PaginatorData = Paginator.GetPageData(cities, page, size);

            IEnumerable<City> paginatedItems = Paginator.Paginate(cities, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search ==null)
            {
                cities = paginatedItems.ToList();
            }

            if (searchBy == "Name")
            {

                if (search == null)
                {

                }
                else
                {
                    search = search.ToLower();
                    cities = cities.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(cities.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(cities.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        cities = paginatedItems.ToList();
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
                    cities = cities.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(cities.OrderBy(x => x.Name), page, size);
                    paginatedItems = Paginator.Paginate(cities.OrderBy(x => x.Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        cities = paginatedItems.ToList();
                    }
                }
            }

            return View(cities);
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .SingleOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // GET: Cities/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.District, "Id", "Name");
            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,DistrictId")] City city)
        {
            
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {

                        _context.Add(city);
                        await _context.SaveChangesAsync();

                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "CityAdded", city);

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
           
            return View(city);
        }

        // GET: Cities/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City.SingleOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            
            return View(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,DistrictId")] City city)
        {
        
            if (id != city.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(city);
                        await _context.SaveChangesAsync();
                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "CityEdited", city);

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CityExists(city.Id))
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
            
            return View(city);
        }

        // GET: Cities/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .SingleOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }
        // GET: Cities/Activate/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.City
                .SingleOrDefaultAsync(m => m.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            return View(city);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var city = await _context.City.SingleOrDefaultAsync(m => m.Id == id);
                    city.Status = "Active";
                    _context.City.Update(city);
                    await _context.SaveChangesAsync();
                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "CityActivated", city);

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
        // POST: Cities/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var city = await _context.City.SingleOrDefaultAsync(m => m.Id == id);
                    city.Status = "InActive";
                    _context.City.Update(city);
                    await _context.SaveChangesAsync();
                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "CityDeactivated", city);

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

        private bool CityExists(int id)
        {
            return _context.City.Any(e => e.Id == id);
        }
    }
}
