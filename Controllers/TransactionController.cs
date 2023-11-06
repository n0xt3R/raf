using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bfws.Data;
using bfws.Helpers;
using bfws.Models;
using bfws.Models.DBModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace bfws.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TransactionController : Controller
    {
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public TransactionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: transaction
        public ActionResult Index(int? page, int? size, int? type, DateTime date)
        {
            // Get our items
            var transactions = _context.Transaction.Include(t => t.ApplicationUser).AsEnumerable();
            if (type != null)
            {
                // Filter by type
                transactions = transactions.Where(t => t.TransactionTypeId == type);
            }
            var test = date.Date.ToString();
            if (date.Date.ToString() != null && date.Date.ToString() != "01/01/0001 12:00:00 AM")
            {
                // Filter by user id
                transactions = transactions.Where(t => t.DateOccurred.Date == date.Date);
            }
            transactions = transactions.OrderByDescending(t => t.DateOccurred);

            // Paginate
            var Paginator = new Paginator<Transaction>();
            ViewBag.PaginatorData = Paginator.GetPageData(transactions, page, size);
            transactions = Paginator.Paginate(transactions, ViewBag.PaginatorData["Page"], ViewBag.PaginatorData["Size"]);

            // Get dropdown lists
            ViewBag.TransactionTypeList = new SelectList(_context.TransactionType.ToList(), "Id", "Name", type ?? 0);
            //ViewBag.UsernameList = new SelectList(_userManager.Users.ToList(), "Id", "UserName", user);

            // Return view model with filtered items
            return View(transactions);
        }

        // GET: transaction/details/5
        public ActionResult Details(int id)
        {
            Transaction transaction = _context.Transaction.Include(t => t.ApplicationUser).Include(t => t.TransactionType).Where(t => t.Id.Equals(id)).First();
            return View(transaction);
        }
    }
}