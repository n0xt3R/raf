using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bfws.Models;
using Microsoft.AspNetCore.Authorization;
using bfws.Data;
using Microsoft.AspNetCore.Identity;
using bfws.Models.HomeViewModels;
using bfws.Models.DBModels;
using Microsoft.EntityFrameworkCore;

namespace bfws.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Current logged in user
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            // Pending Registrations
            List<int> userLevels = new List<int>();
            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Contains("Admin"))
            {
                userLevels.Add(1);
                userLevels.Add(2);
                userLevels.Add(3);
                userLevels.Add(4);
            }
            else
            {
                if (userRoles.Contains("Level1"))
                {
                    userLevels.Add(1);
                }
                if (userRoles.Contains("Level2"))
                {
                    userLevels.Add(2);
                }
                if (userRoles.Contains("Level3"))
                {
                    userLevels.Add(3);
                }
                if (userRoles.Contains("Level4"))
                {
                    userLevels.Add(4);
                }
            }


            // accounts created this month
            int accountsCount = _context.Clients.Where(e =>e.DateCreated.Month.Equals(DateTime.Now.Month)).Count();
           //bills created this month
            int billCount = _context.WaterBills.Where(e => e.DateCreated.Month.Equals(DateTime.Now.Month)).Count();

            // fully paid bills this month
            int fullyPaidCount = _context.WaterBills.Where(e => e.DateCreated.Month.Equals(DateTime.Now.Month)&& e.BillStatus.Name== "Paid in Full").Count();

            //partially paid this month
            int partiallyPaidCount = _context.WaterBills.Where(e => e.DateCreated.Month.Equals(DateTime.Now.Month) && e.BillStatus.Name == "Partially Paid").Count();
            
            HomeViewModel vm = new HomeViewModel
            {
                PersonName = user.FirstName,
                AccountCount= accountsCount,
                FullyPaidCount= fullyPaidCount,
                PartiallyPaidCount= partiallyPaidCount,
                BillCount =billCount

        };
            return View(vm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
