using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class BillPayment
    {
        [Key]
        public int Id { set; get; }
        [DataType(DataType.Currency)]
        public double Payment{set; get;}
        public string CreatedBy { set; get; }
        [DataType(DataType.Date)]
        public DateTime PaymentMadeOn { set; get; }
        public string LastEditedBy { set; get; }
        public int WaterBillId { set; get; }
        public WaterBill WaterBill { set; get; }

        public BillPayment()
        {
            PaymentMadeOn = DateTime.UtcNow;
        }
    }
}
