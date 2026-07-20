using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Domain.Entities
{
    public class BankAccount
    {
        public Guid Id { get;  set; }
        public string UserId { get;  set; } = null!;
        public string AccountNumber { get;  set; } = null!;
        public decimal Balance { get;  set; }
        public DateTime CreatedAt { get;  set; }


    }
}
