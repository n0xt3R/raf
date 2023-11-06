using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class Meter
    {
        [Key]
        public int Id { set; get; }
        public string MeterNo { set; get; }
        public bool Availability { set; get; }
        public string Status { set; get; }

        public Meter()
        {
            Availability = true;
            Status = "Active";
        }
    }
}
