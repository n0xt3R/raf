using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class BillStatus
    {
        [Key]
        public int Id { set; get; }
        public string Name{set; get;}
    }
}
