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
using Microsoft.AspNetCore.Identity;
using bfws.Models;
using bfws.Services;
using Microsoft.AspNetCore.Hosting;

namespace bfws.Controllers
{
    public class StreetsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITransactionLogger _transactionLogger;

        public StreetsController(ApplicationDbContext context,UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ITransactionLogger transactionLogger)
        {
            _context = context;
            _userManager = userManager;
            _transactionLogger = transactionLogger;
        }

        // GET: Streets
        public async Task<IActionResult> Index(int? page, int? size, string searchBy, string search, string query)
        {
            var streets = await _context.Streets.ToListAsync();
            var Paginator = new Paginator<Street>();
            ViewBag.PaginatorData = Paginator.GetPageData(streets.OrderBy(x=>x.Street_Name), page, size);
            IEnumerable<Street> paginatedItems = Paginator.Paginate(streets.OrderBy(x => x.Street_Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
            if (paginatedItems != null && paginatedItems.Count() > 0 && search == null)
            {
                streets = paginatedItems.ToList();
            }
          
            if (searchBy == "Name")
            {
                if (search == null)
                {

                }
                else
                {
                    streets = streets.Where(x => x.Street_Name.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(streets.OrderBy(x => x.Street_Name), page, size);
                    paginatedItems = Paginator.Paginate(streets.OrderBy(x => x.Street_Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        streets = paginatedItems.ToList();
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
                    streets = streets.Where(x => x.Status.ToLower().Contains(search.ToLower())).ToList();
                    ViewBag.PaginatorData = Paginator.GetPageData(streets.OrderBy(x => x.Street_Name), page, size);
                    paginatedItems = Paginator.Paginate(streets.OrderBy(x => x.Street_Name), ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);
                    if (paginatedItems != null && paginatedItems.Count() > 0)
                    {
                        streets = paginatedItems.ToList();
                    }
                }
            }
            return View(streets);
        }

        // GET: Streets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var street = await _context.Streets
                .SingleOrDefaultAsync(m => m.ID == id);

            if (street == null)
            {
                return NotFound();
            }

            return View(street);
        }

        // GET: Streets/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Streets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Street_Name,PollingAreaID,DivisionID")] Street street)
        {
            if (ModelState.IsValid)
            {
           
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Add(street);
                        await _context.SaveChangesAsync();
                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "StreetAdded", street);

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
        
            return View(street);
        }

        // GET: Streets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var street = await _context.Streets.SingleOrDefaultAsync(m => m.ID == id);
            if (street == null)
            {
                return NotFound();
            }

            return View(street);
        }

        // POST: Streets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Street_Name,PollingAreaID,DivisionID")] Street street)
        {
            if (id != street.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        _context.Update(street);
                        await _context.SaveChangesAsync();
                        // Log the transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "StreetEdited", street);

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!StreetExists(street.ID))
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
            }

            return View(street);
        }

        // GET: Streets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeActivate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var street = await _context.Streets
                .SingleOrDefaultAsync(m => m.ID == id);

            if (street == null)
            {
                return NotFound();
            }

            return View(street);
        }

        /*Testing exporting to excel
        public ActionResult ExportToExcel()
        {
            var gv = new GridView();
            gv.DataSource = this.GetEmployeeList();
            gv.DataBind();

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

            gv.RenderControl(objHtmlTextWriter);

            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();

            return View("ExportToExcel");
       
        Testing exporting to excel*/

        // POST: Streets/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeActivate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeActivate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var street = await _context.Streets.SingleOrDefaultAsync(m => m.ID == id);
                    street.Status = "InActive";
                    _context.Update(street);
                    await _context.SaveChangesAsync();
                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "StreetDeactivated", street);

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
        // GET: Streets/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var street = await _context.Streets
                .SingleOrDefaultAsync(m => m.ID == id);

            if (street == null)
            {
                return NotFound();
            }

            return View(street);
        }

        // POST: Streets/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var street = await _context.Streets.SingleOrDefaultAsync(m => m.ID == id);
                    street.Status = "Active";
                    _context.Update(street);
                    await _context.SaveChangesAsync();
                    // Log the transaction
                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "StreetActivated", street);

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
        
        private bool StreetExists(int id)
        {
            return _context.Streets.Any(e => e.ID == id);
        }
    }
}
