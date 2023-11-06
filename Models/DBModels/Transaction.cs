using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bfws.Models.DBModels
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TransactionTypeId { get; set; }
        public TransactionType TransactionType { get; set; }

        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime DateOccurred { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Data { get; set; }
    }
}
