using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using bfws.Services;
using Microsoft.AspNetCore.Identity;
using bfws.Models;
using bfws.Helpers;

namespace bfws.Controllers
{
    public class BillRatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillRatesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: BillRates
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var billRates = await _context.BillRate.ToListAsync();
            var Paginator = new Paginator<BillRate>();
            ViewBag.PaginatorData = Paginator.GetPageData(billRates, page, size);

            IEnumerable<BillRate> paginatedItems = Paginator.Paginate(billRates, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                billRates = paginatedItems.ToList();
            }

            return View(billRates);
        }

        // GET: BillRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billRate = await _context.BillRate
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billRate == null)
            {
                return NotFound();
            }

            return View(billRate);
        }

        //// GET: BillRates/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: BillRates/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Rate,MeterRental")] BillRate billRate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(billRate);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(billRate);
        //}

        // GET: BillRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billRate = await _context.BillRate.SingleOrDefaultAsync(m => m.Id == id);
            if (billRate == null)
            {
                return NotFound();
            }
            return View(billRate);
        }

        // POST: BillRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Rate,MeterRental")] BillRate billRate)
        {
            if (id != billRate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillRateExists(billRate.Id))
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
            return View(billRate);
        }

        // GET: BillRates/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var billRate = await _context.BillRate
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (billRate == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(billRate);
        //}

        //// POST: BillRates/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var billRate = await _context.BillRate.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.BillRate.Remove(billRate);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool BillRateExists(int id)
        {
            return _context.BillRate.Any(e => e.Id == id);
        }
    }
}
