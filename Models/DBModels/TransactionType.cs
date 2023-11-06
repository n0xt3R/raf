﻿using System.ComponentModel.DataAnnotations;

namespace bfws.Models.DBModels
{
    public class TransactionType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
