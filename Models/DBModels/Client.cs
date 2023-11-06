using bfws.Models.DBModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class Client
    {
            [Key]
            public int Id { set; get; }

            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }


            [Display(Name = "Other Names")]
            public string OtherNames { get; set; }

            [Required]
            [Remote(action: "MinimumGenderZero", controller: "Manage")]
            [Display(Name = "Street")]
            public int StreetId { get; set; }

            [ForeignKey("StreetId")]
            public Street Street { get; set; }


        [Display(Name = "Cell Phone")]
            public string CellPhone { get; set; }

            [Required]
            [Remote(action: "MinimumGenderZero", controller: "Manage")]
            [Display(Name = "Gender")]
            public int GenderId { get; set; }

            [ForeignKey("GenderId")]
            public Gender Gender { get; set; }


            [Required]
            public DateTime DateCreated { get; set; }
            public string Status { set; get; }

            public string FullName { set; get; }


        public Client()
            {
                DateCreated = DateTime.UtcNow;
                Status = "Active";
                FullName = FirstName +" "+ OtherNames + " " + LastName;
        }
   

}
}
