using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.WaterBillViewModel
{
    public class BillViewModel
    {
        public DateTime DateCreated { set; get; }
        [DataType(DataType.Date)]
        public string BillMonth { set; get; }
        [DataType(DataType.Date)]
        public DateTime FromBillingDate { set; get; }
        [DataType(DataType.Date)]
        public DateTime ToBillingDate { set; get; }
        [DataType(DataType.Date)]
        public DateTime DueDate { set; get; }
        public int NumberofGallsConsumed { get; set; }
        public string BillCreatedBy { get; set; }
        public string BillLastEditBy { get; set; }

        [DataType(DataType.Currency)]
        public double AmountOwed { set; get; }
        [DataType(DataType.Currency)]
        public double AmountOutstanding { set; get; }

        [Required]
        [Remote(action: "MinimumGenderZero", controller: "Manage")]
        [Display(Name = "Rate")]
        public int RateId { get; set; }

        public int ClientAccountId { set; get; }
    }
}
