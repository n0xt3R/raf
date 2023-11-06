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
using bfws.Services;
using bfws.Models.WaterBillViewModel;
using bfws.Helpers;

namespace bfws.Controllers
{
    public class WaterBillsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public WaterBillsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: WaterBills
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var waterbills = await _context.WaterBills.Include(w => w.BillRate).Include(x => x.BillStatus).Include(x => x.clientAccount).Include(x => x.clientAccount.Client).ToListAsync();
            var Paginator = new Paginator<WaterBill>();
            ViewBag.PaginatorData = Paginator.GetPageData(waterbills, page, size);

            IEnumerable<WaterBill> paginatedItems = Paginator.Paginate(waterbills, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
               waterbills = paginatedItems.ToList();
            }
            
            return View(waterbills);
        }

        public async Task<IActionResult> NewBills(int? page, int? size, string searchBy, string search, string query)
        {
            var waterbills = await _context.WaterBills.Where(x=>x.BillStatus.Name=="New").Include(w => w.BillRate).Include(x => x.BillStatus).Include(x => x.clientAccount).Include(x => x.clientAccount.Client).ToListAsync();
            var Paginator = new Paginator<WaterBill>();
            ViewBag.PaginatorData = Paginator.GetPageData(waterbills, page, size);

            IEnumerable<WaterBill> paginatedItems = Paginator.Paginate(waterbills, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                waterbills = paginatedItems.ToList();
            }

            return View(waterbills);
        }

        public async Task<IActionResult> FullyPaid(int? page, int? size, string searchBy, string search, string query)
        {
            var waterbills = await _context.WaterBills.Where(x => x.BillStatus.Name == "Paid in Full").Include(w => w.BillRate).Include(x => x.BillStatus).Include(x => x.clientAccount).Include(x => x.clientAccount.Client).ToListAsync();
            var Paginator = new Paginator<WaterBill>();
            ViewBag.PaginatorData = Paginator.GetPageData(waterbills, page, size);

            IEnumerable<WaterBill> paginatedItems = Paginator.Paginate(waterbills, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                waterbills = paginatedItems.ToList();
            }

            return View(waterbills);
        }

        public async Task<IActionResult> PartlyPaid(int? page, int? size, string searchBy, string search, string query)
        {
            var waterbills = await _context.WaterBills.Where(x => x.BillStatus.Name == "Partially Paid").Include(w => w.BillRate).Include(x => x.BillStatus).Include(x => x.clientAccount).Include(x => x.clientAccount.Client).ToListAsync();
            var Paginator = new Paginator<WaterBill>();
            ViewBag.PaginatorData = Paginator.GetPageData(waterbills, page, size);

            IEnumerable<WaterBill> paginatedItems = Paginator.Paginate(waterbills, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                waterbills = paginatedItems.ToList();
            }

            return View(waterbills);
        }

        // GET: WaterBills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBills
                .Include(w => w.BillRate)
                .Include(w => w.BillStatus)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterBill == null)
            {
                return NotFound();
            }

            return View(waterBill);
        }

        // GET: WaterBills/Create
        public IActionResult Create(int id)
        {
            BillViewModel model = new BillViewModel();
            model.AmountOutstanding = 0;
            model.AmountOwed = 0;
            model.BillMonth = DateTime.UtcNow.Month.ToString();
            model.FromBillingDate = DateTime.UtcNow;
            model.ToBillingDate = DateTime.UtcNow;
            model.DueDate = DateTime.UtcNow;
            model.NumberofGallsConsumed = 0;
            model.RateId = _context.BillRate.FirstOrDefault().Id;
            model.ClientAccountId = id;

            return View(model);
        }

        // POST: WaterBills/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BillViewModel model)
        {
            WaterBill waterBill = new WaterBill();
            waterBill.NumberofGallsConsumed = model.NumberofGallsConsumed;
            waterBill.FromBillingDate = model.FromBillingDate;
            waterBill.ToBillingDate = model.ToBillingDate;
            waterBill.DueDate = model.DueDate;
            waterBill.BillMonth = model.BillMonth;
            waterBill.clientAccount = _context.ClientAccounts.Where(x=>x.ID==model.ClientAccountId).FirstOrDefault();
            waterBill.BillRate = _context.BillRate.Where(x=>x.Id==model.RateId).SingleOrDefault();

            waterBill.AmountOwed = waterBill.BillRate.Rate * waterBill.NumberofGallsConsumed + waterBill.BillRate.MeterRental;
            waterBill.AmountOwed = Math.Round(waterBill.AmountOwed,2);
            if(waterBill.AmountOwed > 100.0)
            {
                waterBill.AmountOwed = (waterBill.AmountOwed * waterBill.BillRate.GST) / 100 + waterBill.AmountOwed;
            }

            waterBill.AmountOutstanding = waterBill.AmountOwed;
            var user = await _userManager.GetUserAsync(HttpContext.User);
            waterBill.BillCreatedBy = user.UserName;
            waterBill.BillLastEditBy = user.UserName;

            if (ModelState.IsValid)
            {
                _context.Add(waterBill);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","ClientAccounts");
            }
            //ViewData["RateId"] = new SelectList(_context.BillRate, "Id", "Rate", waterBill.RateId);

            return View(model);
        }

        // GET: WaterBills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBills.SingleOrDefaultAsync(m => m.Id == id);
            if (waterBill == null)
            {
                return NotFound();
            }
            
            return View(waterBill);
        }

        // POST: WaterBills/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BillMonth,FromBillingDate,ToBillingDate,DueDate,NumberofGallsConsumed")] WaterBill waterBill)
        {
            if (id != waterBill.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waterBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterBillExists(waterBill.Id))
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
            ViewData["RateId"] = new SelectList(_context.BillRate, "Id", "Rate", waterBill.RateId);
            return View(waterBill);
        }

        // GET: WaterBills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBills
                .Include(w => w.BillRate)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (waterBill == null)
            {
                return NotFound();
            }

            return View(waterBill);
        }

        // POST: WaterBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterBill = await _context.WaterBills.SingleOrDefaultAsync(m => m.Id == id);
            _context.WaterBills.Remove(waterBill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterBillExists(int id)
        {
            return _context.WaterBills.Any(e => e.Id == id);
        }
    }
}
