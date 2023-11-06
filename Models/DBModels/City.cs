using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class City //holds cities and Towns
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { set; get; }

        public City()
        {
            Status = "Active";
        }
    }
}
