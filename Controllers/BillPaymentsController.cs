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
using bfws.Models;
using Microsoft.AspNetCore.Identity;
using bfws.Models.BillPaymentViewModels;

namespace bfws.Controllers
{
    public class BillPaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillPaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: BillPayments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BillPayments.Include(b => b.WaterBill);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BillPayments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPayment = await _context.BillPayments
                .Include(b => b.WaterBill)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billPayment == null)
            {
                return NotFound();
            }

            return View(billPayment);
        }

        // GET: BillPayments/Create
        public IActionResult Create(int id)
        {
            CreateBillPaymentViewModel model = new CreateBillPaymentViewModel();
            model.WaterBillId = id;

            var bill = _context.WaterBills.SingleOrDefault(m=>m.Id==id);
            model.OutStandingBalance = bill.AmountOutstanding;
            
            return View(model);
        }

        // POST: BillPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBillPaymentViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            BillPayment billPayment = new BillPayment();
            billPayment.Payment = model.Payment;
            billPayment.CreatedBy = user.UserName;
            billPayment.LastEditedBy = user.UserName;

            WaterBill waterBill = new WaterBill();
            waterBill = await _context.WaterBills.Include(x=>x.BillStatus).SingleOrDefaultAsync(m=>m.Id==model.WaterBillId);

            waterBill.AmountOutstanding = waterBill.AmountOutstanding - model.Payment;
            if(waterBill.AmountOutstanding <0)
            {
                waterBill.BillStatusId = 2;
            }
            else
            {
                waterBill.BillStatusId = 1;
            }
            billPayment.WaterBillId = waterBill.Id;

            if (ModelState.IsValid)
            {
                _context.Add(billPayment);
                await _context.SaveChangesAsync();
                int ID = billPayment.Id;
                _context.Update(waterBill);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = ID });
            }
            
            return View(model);
        }

        // GET: BillPayments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPayment = await _context.BillPayments.Include(x=>x.WaterBill).SingleOrDefaultAsync(m => m.Id == id);
            if (billPayment == null)
            {
                return NotFound();
            }

            EditBillPaymentViewModel model = new EditBillPaymentViewModel();
            model.Id = billPayment.Id;
            model.Payment = billPayment.Payment;
            model.WaterBillId = billPayment.WaterBillId;
            model.OutStandingBalance = billPayment.WaterBill.AmountOwed;

            return View(model);
        }

        // POST: BillPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditBillPaymentViewModel model)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var billPayment = _context.BillPayments.SingleOrDefault(m=>m.Id == model.Id);
            billPayment.Payment = model.Payment;
            billPayment.CreatedBy = user.UserName;
            billPayment.LastEditedBy = user.UserName;

            WaterBill waterBill = new WaterBill();
            waterBill = await _context.WaterBills.Include(x => x.BillStatus).SingleOrDefaultAsync(m => m.Id == model.WaterBillId);
            waterBill.AmountOutstanding = waterBill.AmountOwed;
            waterBill.AmountOutstanding = waterBill.AmountOutstanding - model.Payment;
            if (waterBill.AmountOutstanding < 0)
            {
                waterBill.BillStatusId = 2;
            }
            else
            {
                waterBill.BillStatusId = 1;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(billPayment);
                    await _context.SaveChangesAsync();
                    _context.Update(waterBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BillPaymentExists(billPayment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = billPayment.Id });
            }

            return View(model);
        }

        // GET: BillPayments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var billPayment = await _context.BillPayments
                .Include(b => b.WaterBill)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (billPayment == null)
            {
                return NotFound();
            }

            return View(billPayment);
        }

        // POST: BillPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var billPayment = await _context.BillPayments.SingleOrDefaultAsync(m => m.Id == id);
            _context.BillPayments.Remove(billPayment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BillPaymentExists(int id)
        {
            return _context.BillPayments.Any(e => e.Id == id);
        }
    }
}
