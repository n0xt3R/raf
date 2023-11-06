using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bfws.Models.ManageViewModels
{
    public class EditUserViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Other Names")]
        public string OtherNames { get; set; }

        [Required]
        [Display(Name = "District")]
        public int? DistrictId { get; set; }


        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Cell Phone")]
        [Phone]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "City")]
        public int? CityId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Required]
        [Display(Name = "Home Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Display(Name ="New Password")]
        public string NewPassword { get; set; }

        public List<RolePermissionViewModel> RoleList { get; set; } // contains select and roles 
        public SelectList DistrictSelectList { get; set; }
        public SelectList CitySelectList { get; set; }
        public SelectList GenderSelectList { get; set; }
        public SelectList StreetSelectlist { set; get; }

        public EditUserViewModel() { }

        public EditUserViewModel(ApplicationUser user)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            OtherNames = user.OtherNames;
            JobTitle = user.JobTitle;
            Street = user.Street;
            CellPhone = user.CellPhone;
            CityId = user.CityId;
            GenderId = user.GenderId;
            PhoneNumber = user.PhoneNumber;
            Email = user.Email;
            LockoutEnabled = user.LockoutEnabled;

        }
    }
}
