using BFWS.Models.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bfws.Models.DBModels
{
    public class ClientAccount
    {
        [Key]
        public int ID { set; get; }
        public string AccountNo { set; get; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Required]
        [Display(Name = "Meter")]
        public int MeterId { get; set; }

        [ForeignKey("MeterId")]
        public Meter Meter { get; set; }

        [Required]
        [Display(Name = "Account Status")]
        public int AccountStatusId { get; set; }

        [ForeignKey("AccountStatusId")]
        public AccountStatus AccountStatus{ get; set; }

        public ClientAccount()
        {
            AccountStatusId = 1;
        }
    }
}
