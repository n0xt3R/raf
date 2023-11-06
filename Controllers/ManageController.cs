using bfws.Data;
using bfws.Models;
using bfws.Models.DBModels;
using bfws.Models.ManageViewModels;
using bfws.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace bfws.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
        private readonly ApplicationDbContext _context;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);
        private readonly ITransactionLogger _transactionLogger;

        public ManageController(
           ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender,
          ILogger<ManageController> logger,
         ITransactionLogger transactionLogger,
         UrlEncoder urlEncoder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
            _context = context;
            _transactionLogger = transactionLogger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            ApplicationUser user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{id}'.");
            }

            EditUserViewModel model = new EditUserViewModel(user)
            {
                RoleList = _context.Roles.OrderBy(m => m.NormalizedName).Select(m => new RolePermissionViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            };
            model.DistrictSelectList = new SelectList(_context.District.ToList(), "Id", "Name", model.DistrictId);
            model.CitySelectList = new SelectList(_context.City.Where(x => x.Status == "Active").ToList(), "Id", "Name", model.CityId);
            model.GenderSelectList = new SelectList(_context.Gender.ToList(), "Id", "Name", model.GenderId);

            foreach (var role in model.RoleList)
            {
                role.Selected = await _userManager.IsInRoleAsync(user, role.Name);
            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ApplicationUserExists(model.UserId))
            {
                throw new ApplicationException($"Unable to load user with ID '{model.UserId}'.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Grab old user info for transaction logging
                    ApplicationUser oldUser = _context.Users.AsNoTracking().Single(u => u.Id == model.UserId);
                    IList<string> oldRoles = await _userManager.GetRolesAsync(oldUser);

                    // Update new user
                    ApplicationUser user = _context.Users.Find(model.UserId);
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.OtherNames = model.OtherNames;
                    user.DistrictId = model.DistrictId ?? 0;
                    user.JobTitle = model.JobTitle;
                    user.Street = model.Street;
                    user.CellPhone = model.CellPhone;
                    user.CityId = model.CityId ?? 0;
                    user.GenderId = model.GenderId ?? 0;
 
                    user.PhoneNumber = model.PhoneNumber;
                    if (!String.IsNullOrEmpty(model.Email))
                    {
                        var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                        if (!setEmailResult.Succeeded)
                        {
                            AddErrors(setEmailResult);
                        }
                    }
                    if (!String.IsNullOrEmpty(model.NewPassword))
                    {
                        // New password set
                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                        await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    }
                    // Remove user from all old roles
                    await _userManager.RemoveFromRolesAsync(user, oldRoles);
                    // Add user to new roles
                    await _userManager.AddToRolesAsync(user, model.RoleList.Where(r => r.Selected == true).Select(r => r.Name).AsEnumerable());
                    user.LockoutEnabled = model.LockoutEnabled;
                    // Save DB
                    await _context.SaveChangesAsync();

                    await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "EditUser", new { @new = user, @oldInfo = oldUser, oldRoles, @newRoles = model.RoleList.Select(r => r.Name) });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(model.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["StatusMessage"] = "User Information saved"; // Store status message 
                return RedirectToAction(nameof(Users));
            }

            model.RoleList = _context.Roles.OrderBy(r => r.Name).Select(
                m => new RolePermissionViewModel { Id = m.Id, Name = m.Name }).ToList();
            
            model.DistrictSelectList = new SelectList(_context.District.ToList(), "Id", "Name", model.DistrictId);
            model.CitySelectList = new SelectList(_context.City.Where(x => x.Status == "Active").ToList(), "Id", "Name", model.CityId);
            model.GenderSelectList = new SelectList(_context.Gender.ToList(), "Id", "Name", model.GenderId);

            return View(model);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult NewUser()
        {
            var model = new NewUserViewModel();
            model.RoleList = _context.Roles.OrderBy(r => r.Name).Select(
                m => new RolePermissionViewModel { Id = m.Id, Name = m.Name }).ToList();
            
            model.DistrictSelectList = new SelectList(_context.District.ToList(), "Id", "Name");
            model.CitySelectList = new SelectList(_context.City.Where(x => x.Status == "Active").ToList(), "Id", "Name");
            model.GenderSelectList = new SelectList(_context.Gender.ToList(), "Id", "Name");
           

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewUser(NewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Construct user
                    ApplicationUser user = new ApplicationUser();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.OtherNames = model.OtherNames;
                    user.DistrictId = model.DistrictId;
                    user.JobTitle = model.JobTitle;
                    user.Street = model.Street;
                    user.CellPhone = model.CellPhone;
                    user.PhoneNumber = model.HomeNumber;
                    user.CityId = model.CityId;
                    user.GenderId = model.GenderId;
                    user.UserName = user.Email;
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.NormalizedUserName = model.Email.ToUpper();
                    user.NormalizedEmail = model.Email.ToUpper();

                    // Create user in database
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        // Add user to roles selected
                        await _userManager.AddToRolesAsync(user, model.RoleList.Where(r => r.Selected == true).Select(r => r.Name).AsEnumerable());

                        // Log transaction
                        await _transactionLogger.LogTransaction(_context, await _userManager.GetUserAsync(HttpContext.User), "AddUser", new { user, @roles = await _userManager.GetRolesAsync(user) });
                        TempData["StatusMessage"] = "User was created successfully";
                        return RedirectToAction(nameof(Users));
                    }
                    AddErrors(result);
                }
                catch (DbUpdateException)
                {
                    //Log the error (uncomment ex variable name and write a log.
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists " +
                        "see your system administrator.");
                }
            }

           
            model.DistrictSelectList = new SelectList(_context.District.ToList(), "Id", "Name", model.DistrictId);
            model.CitySelectList = new SelectList(_context.City.Where(x => x.Status == "Active").ToList(), "Id", "Name", model.CityId);
            model.GenderSelectList = new SelectList(_context.Gender.ToList(), "Id", "Name", model.GenderId);
           
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Users(string sortOrder, string currentFilter, string searchString, int? page)
        {
          //  List<ApplicationUser> useList = new List<ApplicationUser>();

            var DistrictList = _context.District.ToList();

            DistrictList.Insert(0, new District { Id = 0, Name = "Select District" });

            ViewBag.DistrictList = DistrictList;

            var CityList = _context.City.Where(x => x.Status == "Active").ToList();

            CityList.Insert(0, new City { Id = 0, Name = "Select City" });

            ViewBag.CityList = CityList;


            ViewData["CurrentSort"] = sortOrder;

            ViewData["FirstNameSortParm"] = sortOrder == "first_name_desc" ? "first_name_asc" : "first_name_desc";
            ViewData["LastNameSortParm"] = sortOrder == "last_name_desc" ? "last_name_asc" : "last_name_desc";
            ViewData["JobTitleSortParm"] = sortOrder == "job_title_desc" ? "job_title_asc" : "job_title_desc";
            ViewData["DateSortParm"] = sortOrder == "date_asc" ? "date_desc" : "date_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var app_users = from s in _context.Users
                            select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                app_users = app_users.Where(s => s.LastName.Contains(searchString)
                                            || s.FirstName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "first_name_desc":
                    app_users = app_users.OrderByDescending(s => s.FirstName);
                    break;
                case "first_name_asc":
                    app_users = app_users.OrderBy(s => s.FirstName);
                    break;
                case "last_name_desc":
                    app_users = app_users.OrderByDescending(s => s.LastName);
                    break;
                case "last_name_asc":
                    app_users = app_users.OrderBy(s => s.LastName);
                    break;
                case "job_title_desc":
                    app_users = app_users.OrderByDescending(s => s.JobTitle);
                    break;
                case "job_title_asc":
                    app_users = app_users.OrderBy(s => s.JobTitle);
                    break;
                case "date_desc":
                    app_users = app_users.OrderByDescending(s => s.DateCreated);
                    break;
                case "date_asc":
                    app_users = app_users.OrderBy(s => s.DateCreated);
                    break;
                default:
                    app_users = app_users.OrderBy(s => s.FirstName);
                    break;
            }
            return View(app_users);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        #endregion
    }
}
