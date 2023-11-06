using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class WaterBill
    {
        [Key]
        public int Id { set; get; }
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

        [ForeignKey("RateId")]
        public BillRate BillRate { get; set; }

        [Required]
        [Remote(action: "MinimumGenderZero", controller: "Manage")]
        [Display(Name = "Bill Status")]
        public int BillStatusId { get; set; }

        [ForeignKey("BillStatusId")]
        public BillStatus BillStatus { get; set; }

        public int ClientAccountId { set; get; }
        [ForeignKey("ClientAccountId")]
        public ClientAccount clientAccount { set; get; }

        public List<BillPayment> Payments{set; get;}


        public WaterBill()
        {
            DateCreated = DateTime.Now;
            BillMonth = (DateCreated.Month - 1).ToString();
            RateId = 1;
            BillStatusId = 3;
        }
    }
}
