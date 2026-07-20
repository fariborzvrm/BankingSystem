using BankingSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}
