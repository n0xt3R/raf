using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bfws.Models.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace bfws.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Other Names")]
        public string OtherNames { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Street Name")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Cell Phone")]
        public string CellPhone { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }

        public City City { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District District { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
        public string Status { set; get; }

        public ApplicationUser()
        {
            DateCreated = DateTime.Now;
            Status = "Active";
        }
    }
}
