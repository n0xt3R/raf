using bfws.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.HomeViewModels
{
    public class HomeViewModel
    {
        public string PersonName { get; set; } 
        // Data section
        public int AccountCount { get; set; }
        public int FullyPaidCount { get; set; }
        public int PartiallyPaidCount { get; set; }
        public int BillCount { get; set; }
        // Approve Reg Section
    }
}
