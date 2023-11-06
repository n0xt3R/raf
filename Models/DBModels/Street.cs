using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class Street
    {
        [Key]
        public int ID { set; get; }
        public string Street_Name { set; get; }

        public string Status { set; get; }

        public Street()
        {
            Status = "Active";
        }
    }
}
