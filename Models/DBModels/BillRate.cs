using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class BillRate
    {
        [Key]
        public int Id { set; get; }
        public double Rate { set; get; }
        public double MeterRental { set; get; }
        public double GST { set; get; }
    }
}
