using bfws.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.WaterBillViewModel
{
    public class ClientBillsVM
    {
        public ClientAccount ClientAccount { set; get; }
        public List<WaterBill> ClientBills { set; get; }
        public List<BillPayment> Payments { set; get; }
    }
}
