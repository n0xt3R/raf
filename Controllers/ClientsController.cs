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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ITransactionLogger _transactionLogger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
         ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: Clients
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var clients = await _context.Clients.Include(c => c.Gender).Include(c => c.Street).ToListAsync();
            var Paginator = new Paginator<Client>();
            ViewBag.PaginatorData = Paginator.GetPageData(clients, page, size);

            IEnumerable<Client> paginatedItems = Paginator.Paginate(clients, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                clients = paginatedItems.ToList();
            }

            return View(clients);

        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Gender)
                .Include(c => c.Street)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["GenderId"] = new SelectList(_context.Gender.Where(x => x.Status == "Active"), "Id", "Name");
            ViewData["StreetId"] = new SelectList(_context.Streets.Where(x => x.Status == "Active"), "ID", "Street_Name");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,OtherNames,StreetId,CellPhone,GenderId")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenderId"] = new SelectList(_context.Gender.Where(x => x.Status == "Active"), "Id", "Name", client.GenderId);
            ViewData["StreetId"] = new SelectList(_context.Streets.Where(x => x.Status == "Active"), "ID", "Street_Name", client.StreetId);
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            ViewData["GenderId"] = new SelectList(_context.Gender.Where(x=>x.Status=="Active"), "Id", "Name", client.GenderId);
            ViewData["StreetId"] = new SelectList(_context.Streets.Where(x=>x.Status=="Active"), "ID", "Street_Name", client.StreetId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,OtherNames,StreetId,CellPhone,GenderId")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
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
            ViewData["GenderId"] = new SelectList(_context.Gender.Where(x => x.Status == "Active"), "Id", "Name", client.GenderId);
            ViewData["StreetId"] = new SelectList(_context.Streets.Where(x => x.Status == "Active"), "ID", "Street_Name", client.StreetId);
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .Include(c => c.Gender)
                .Include(c => c.Street)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _context.Clients.SingleOrDefaultAsync(m => m.Id == id);
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }
}
