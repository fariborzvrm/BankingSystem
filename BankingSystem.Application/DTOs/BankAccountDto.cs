using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.DTOs
{
    public class BankAccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
