﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.BillPaymentViewModels
{
    public class EditBillPaymentViewModel
    {
        public int Id { set; get; }
        [DataType(DataType.Currency)]
        public double OutStandingBalance { set; get; }
        [DataType(DataType.Currency)]
        public double Payment { set; get; }
        public int WaterBillId { set; get; }
    }
}
