using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using bfws.Models.MeterViewModels;
using bfws.Services;
using bfws.Models;
using Microsoft.AspNetCore.Identity;
using bfws.Helpers;

namespace bfws.Controllers
{
    public class MetersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public MetersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: Meters
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var meters = await _context.Meter.ToListAsync();
            var Paginator = new Paginator<Meter>();
            ViewBag.PaginatorData = Paginator.GetPageData(meters, page, size);

            IEnumerable<Meter> paginatedItems = Paginator.Paginate(meters, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                meters = paginatedItems.ToList();
            }

            return View(meters);

        }

        // GET: Meters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meter = await _context.Meter
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }

            return View(meter);
        }

        // GET: Meters/Create
        public IActionResult Create()
        {
            var model = new MeterViewModel();
            model.MeterNo = "BFWS000";

            return View(model);
        }

        // POST: Meters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeterViewModel model)
        {
            var meter = new Meter();
            meter.MeterNo = model.MeterNo;

            if (ModelState.IsValid)
            {
                _context.Add(meter);
                await _context.SaveChangesAsync();
                meter.MeterNo = meter.MeterNo + meter.Id.ToString();
                _context.Update(meter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(meter);
        }

        // GET: Meters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meter = await _context.Meter.SingleOrDefaultAsync(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }
            return View(meter);
        }

        // POST: Meters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MeterNo")] Meter meter)
        {
            if (id != meter.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeterExists(meter.Id))
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
            return View(meter);
        }

        // GET: Meters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meter = await _context.Meter
                .SingleOrDefaultAsync(m => m.Id == id);
            if (meter == null)
            {
                return NotFound();
            }

            return View(meter);
        }

        // POST: Meters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meter = await _context.Meter.SingleOrDefaultAsync(m => m.Id == id);
            _context.Meter.Remove(meter);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeterExists(int id)
        {
            return _context.Meter.Any(e => e.Id == id);
        }
    }
}
