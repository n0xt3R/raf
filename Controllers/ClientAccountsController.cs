using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using bfws.Data;
using bfws.Models.DBModels;
using Remotion.Linq.Clauses;
using bfws.Helpers;
using bfws.Models;
using Microsoft.AspNetCore.Identity;
using bfws.Services;
using bfws.Models.WaterBillViewModel;

namespace bfws.Controllers
{
    public class ClientAccountsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientAccountsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: ClientAccounts
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var clientAccounts = await _context.ClientAccounts.Include(x=>x.AccountStatus).Include(x=>x.Client).ToListAsync();
            var Paginator = new Paginator<ClientAccount>();
            ViewBag.PaginatorData = Paginator.GetPageData(clientAccounts, page, size);

            IEnumerable<ClientAccount> paginatedItems = Paginator.Paginate(clientAccounts, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                clientAccounts = paginatedItems.ToList();
            }
           
            return View(clientAccounts);
        }

        public async Task<IActionResult> ClientList(int? page, int? size, string searchBy, string search, string query)
        {
            var clientAccounts = await _context.ClientAccounts.Where(x => x.AccountStatus.Name == "Active").Include(x => x.AccountStatus).Include(x => x.Client).Include(x=>x.Meter).ToListAsync();
            var Paginator = new Paginator<ClientAccount>();
            ViewBag.PaginatorData = Paginator.GetPageData(clientAccounts, page, size);

            IEnumerable<ClientAccount> paginatedItems = Paginator.Paginate(clientAccounts, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                clientAccounts = paginatedItems.ToList();
            }

            return View(clientAccounts);
        }

        public async Task<IActionResult> ClientBills(int? page, int? size, string searchBy, string search, string query)
        {
            var clientAccounts = await _context.ClientAccounts.Where(x => x.AccountStatus.Name == "Active").Include(x => x.AccountStatus).Include(x => x.Client).Include(x => x.Meter).ToListAsync();
            var Paginator = new Paginator<ClientAccount>();
            ViewBag.PaginatorData = Paginator.GetPageData(clientAccounts, page, size);

            IEnumerable<ClientAccount> paginatedItems = Paginator.Paginate(clientAccounts, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                clientAccounts = paginatedItems.ToList();
            }

            return View(clientAccounts);
        }
        
        public async Task<IActionResult>BillList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientAccount = await _context.ClientAccounts
                .Include(c => c.AccountStatus)
                .Include(c => c.Client)
                .Include(c => c.Meter)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (clientAccount == null)
            {
                return NotFound();
            }
            ClientBillsVM model = new ClientBillsVM();
            model.ClientAccount = new ClientAccount();
            model.ClientAccount = clientAccount;
            model.ClientBills = new List<WaterBill>();
            model.ClientBills =_context.WaterBills.Include(x => x.clientAccount).Include(x=>x.clientAccount.Client).Include(x => x.BillStatus).Where(x => x.clientAccount.ID == id).ToList();
            foreach (var bill in model.ClientBills)
            {
                model.Payments = new List<BillPayment>();
                model.Payments = _context.BillPayments.Include(x => x.WaterBill).Where(x => x.WaterBillId == bill.Id).ToList();
            }
                 
            return View(model);
        }
        

        public async Task<IActionResult> Disconnected(int? page, int? size, string searchBy, string search, string query)
        {
            var clientAccounts = await _context.ClientAccounts.Where(x => x.AccountStatus.Name == "Disconnected").Include(x => x.AccountStatus).Include(x => x.Client).Include(x => x.Meter).ToListAsync();
            var Paginator = new Paginator<ClientAccount>();
            ViewBag.PaginatorData = Paginator.GetPageData(clientAccounts, page, size);

            IEnumerable<ClientAccount> paginatedItems = Paginator.Paginate(clientAccounts, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                clientAccounts = paginatedItems.ToList();
            }

            return View(clientAccounts);
        }

        // GET: ClientAccounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientAccount = await _context.ClientAccounts
                .Include(c => c.AccountStatus)
                .Include(c => c.Client)
                .Include(c => c.Meter)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (clientAccount == null)
            {
                return NotFound();
            }

            return View(clientAccount);
        }

        // GET: ClientAccounts/Create
        public IActionResult Create()
        {
            ViewData["AccountStatusId"] = new SelectList(_context.AccountStatus, "Id", "Id");
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x=>x.Status=="Active"), "Id", "FullName");
            ViewData["MeterId"] = new SelectList(_context.Meter.Where(x=>x.Status=="Active" && x.Availability==true), "Id", "MeterNo");
            return View();
        }

        // POST: ClientAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,AccountNo,ClientId,MeterId")] ClientAccount clientAccount)
        {
            Meter meter = _context.Meter.Where(x=>x.Id == clientAccount.MeterId).SingleOrDefault();

            if (ModelState.IsValid)
            {
                _context.Add(clientAccount);
                await _context.SaveChangesAsync();
                meter.Availability = false;
                _context.Update(meter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountStatusId"] = new SelectList(_context.AccountStatus, "Id", "Id", clientAccount.AccountStatusId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x=>x.Status=="Active"), "Id", "FullName", clientAccount.ClientId);
            ViewData["MeterId"] = new SelectList(_context.Meter.Where(x => x.Status == "Active" && x.Availability == true), "Id", "MeterNo", clientAccount.MeterId);
            return View(clientAccount);
        }

        // GET: ClientAccounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientAccount = await _context.ClientAccounts.SingleOrDefaultAsync(m => m.ID == id);
            if (clientAccount == null)
            {
                return NotFound();
            }
            ViewData["AccountStatusId"] = new SelectList(_context.AccountStatus, "Id", "Id", clientAccount.AccountStatusId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x=>x.Status=="Active"), "Id", "FullName", clientAccount.ClientId);
            ViewData["MeterId"] = new SelectList(_context.Meter.Where(x => x.Status == "Active" && x.Availability == true), "Id", "MeterNo", clientAccount.MeterId);
            return View(clientAccount);
        }

        // POST: ClientAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,AccountNo,ClientId,MeterId")] ClientAccount clientAccount)
        {
            if (id != clientAccount.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(clientAccount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientAccountExists(clientAccount.ID))
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
            ViewData["AccountStatusId"] = new SelectList(_context.AccountStatus, "Id", "Id", clientAccount.AccountStatusId);
            ViewData["ClientId"] = new SelectList(_context.Clients.Where(x=>x.Status=="Active"), "Id", "FullName", clientAccount.ClientId);
            ViewData["MeterId"] = new SelectList(_context.Meter.Where(x => x.Status == "Active" && x.Availability == true), "Id", "MeterNo", clientAccount.MeterId);
            return View(clientAccount);
        }

        // GET: ClientAccounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientAccount = await _context.ClientAccounts
                .Include(c => c.AccountStatus)
                .Include(c => c.Client)
                .Include(c => c.Meter)
                .SingleOrDefaultAsync(m => m.ID == id);
            if (clientAccount == null)
            {
                return NotFound();
            }

            return View(clientAccount);
        }

        // POST: ClientAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var clientAccount = await _context.ClientAccounts.SingleOrDefaultAsync(m => m.ID == id);
            _context.ClientAccounts.Remove(clientAccount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientAccountExists(int id)
        {
            return _context.ClientAccounts.Any(e => e.ID == id);
        }
    }
}
