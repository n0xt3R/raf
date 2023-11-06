using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bfws.Models.ManageViewModels
{
    public class NewUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }


        [Display(Name = "Other Names")]
        public string OtherNames { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        public string Street { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Home Number")]
        public string HomeNumber { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public List<RolePermissionViewModel> RoleList { get; set; } // contains select and roles 
        public SelectList DistrictSelectList { get; set; }
        public SelectList CitySelectList { get; set; }
        public SelectList GenderSelectList { get; set; }
 

    }
}
