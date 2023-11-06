using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BFWS.Models.DBModels
{
    public class AccountStatus
    {
        [Key]
        public int Id { set; get; }
        public string Name { set; get; }
    }
}
