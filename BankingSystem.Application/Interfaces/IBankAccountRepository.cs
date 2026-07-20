using BankingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Interfaces
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> AddAsync(BankAccount bankAccount);
        Task<List<BankAccount>> GetByUserIdAsync(string userId);
        Task<bool> ExistsByAccountNumberAsync(string accountNumber);

    }
}
