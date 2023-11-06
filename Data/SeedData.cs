using bfws.Models;
using bfws.Models.DBModels;
using BFWS.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace bfws.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            context.Database.EnsureCreated();
            // Seed Data
            if (!context.Gender.Any())
            {
                context.Gender.Add(new Gender() { Name = "Male", Status="Active"});
                context.Gender.Add(new Gender() { Name = "Female", Status = "Active" });
                context.Gender.Add(new Gender() { Name = "Other", Status = "Active" });
                context.SaveChanges();
            }

            if (!context.City.Any())
            {
                context.City.Add(new City() { Name = "Belize City" , Status = "Active"});
                context.City.Add(new City() { Name = "Belmopan City", Status = "Active" });
                context.City.Add(new City() { Name = "Other", Status = "Active" });
                context.SaveChanges();
            }

            if (!context.BillRate.Any())
            {
                context.BillRate.Add(new BillRate() { Rate = 0.50, MeterRental = 5.00 , GST= 12.5});
                context.SaveChanges();
            }

            if (!context.Salutation.Any())
            {
                context.Salutation.Add(new Salutation() { Name = "Mr." });
                context.Salutation.Add(new Salutation() { Name = "Ms." });
                context.Salutation.Add(new Salutation() { Name = "Mrs." });
                context.Salutation.Add(new Salutation() { Name = "Dr." });
                context.SaveChanges();
            }

            if (!context.MaritalState.Any())
            {
                context.MaritalState.Add(new MaritalState() { Name = "Single"});
                context.MaritalState.Add(new MaritalState() { Name = "Married"});
                context.MaritalState.Add(new MaritalState() { Name = "Divorced"});
                context.MaritalState.Add(new MaritalState() { Name = "Widowed"});
                context.MaritalState.Add(new MaritalState() { Name = "Common Law"});
                context.SaveChanges();
            }

            if (!context.Streets.Any())
            {
                context.Streets.Add(new Street() { Street_Name = "Street 1"});
                context.Streets.Add(new Street() { Street_Name = "Street 2" });
                context.Streets.Add(new Street() { Street_Name = "Street 3" });
                context.Streets.Add(new Street() { Street_Name = "Street 4" });
                context.SaveChanges();
            }

            if (!context.AccountStatus.Any())
            {
                context.AccountStatus.Add(new AccountStatus() { Name = "Active" });
                context.AccountStatus.Add(new AccountStatus() { Name = "Disconnected" });
                context.SaveChanges();
            }

            if (!context.BillStatus.Any())
            {
                context.BillStatus.Add(new BillStatus() { Name = "Partially Paid" });
                context.BillStatus.Add(new BillStatus() { Name = "Paid in Full" });
                context.BillStatus.Add(new BillStatus() { Name = "New" });
                context.SaveChanges();
            }
            if (!context.District.Any())
            {
                context.District.Add(new District() { Name = "Corozal" });
                context.District.Add(new District() { Name = "Orange Walk" });
                context.District.Add(new District() { Name = "Belize" });
                context.District.Add(new District() { Name = "Cayo" });
                context.District.Add(new District() { Name = "Stann Creek" });
                context.District.Add(new District() { Name = "Toledo" });
                context.District.Add(new District() { Name = "Other" });
                context.SaveChanges();
            }

            if (!context.TransactionType.Any())
            {
                context.TransactionType.Add(new TransactionType { Name = "AddUser" });
                context.TransactionType.Add(new TransactionType { Name = "EditUser" });
                context.TransactionType.Add(new TransactionType { Name = "Login" });
                context.TransactionType.Add(new TransactionType { Name = "SkinColorAdded" });
                context.TransactionType.Add(new TransactionType { Name = "SkinColorEdited" });
                context.TransactionType.Add(new TransactionType { Name = "EyeColorAdded" });
                context.TransactionType.Add(new TransactionType { Name = "EyeColorEdited" });
                context.TransactionType.Add(new TransactionType { Name = "MaritalStatusAdded" });
                context.TransactionType.Add(new TransactionType { Name = "MaritalStatusEdited" });
                context.TransactionType.Add(new TransactionType { Name = "CityAdded" });
                context.TransactionType.Add(new TransactionType { Name = "CityEdited" });
                context.TransactionType.Add(new TransactionType { Name = "CityActivated" });
                context.TransactionType.Add(new TransactionType { Name = "CityDeactivated" });
                context.TransactionType.Add(new TransactionType { Name = "StreetAdded" });
                context.TransactionType.Add(new TransactionType { Name = "StreetEdited" });
                context.TransactionType.Add(new TransactionType { Name = "StreetActivated" });
                context.TransactionType.Add(new TransactionType { Name = "StreetDeactivated" });
                context.SaveChanges();
            }

            if (!context.Roles.Any())
            {
      
                roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Add" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Edit" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Delete" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Browse" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Report" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Download" }).Wait();
                roleManager.CreateAsync(new IdentityRole { Name = "Print" }).Wait();
            }

            var admin = userManager.FindByNameAsync("Administrator").Result;
            if (admin == null)
            {
                // Create our administrator user
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "administrator@bfws",
                    NormalizedUserName = "ADMINISTRATOR@BFWS",
                    Email = "administrator@bfws",
                    EmailConfirmed = true,
                    FirstName = "Administrator",
                    LastName = "Administrator",
                    DistrictId = context.District.First().Id,
                    CityId = context.City.First().Id,
                    GenderId = context.Gender.First().Id,
                    JobTitle = "Administrator",
                    CellPhone = "555-5555",
                    PhoneNumber = "555-5555",
                    Street = "My Street",
                    LockoutEnabled = false
                };
                userManager.CreateAsync(user, "Password1/").Wait();

                // Add the admin to admin role
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}
