using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class MaritalState
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { set; get; }

        public MaritalState()
        {
            Status = "Active";
        }
    }
}
