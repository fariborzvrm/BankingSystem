using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using BankingSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;


namespace BankingSystem.Infrastructure.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public BankAccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> AddAsync(BankAccount bankAccount) {

            _context.BankAccounts.Add(bankAccount);
            return bankAccount;
        }

        public async Task<bool> ExistsByAccountNumberAsync(string accountNumber)
        {
             return await _context.BankAccounts.AsNoTracking().AnyAsync(b => b.AccountNumber == accountNumber);           
        }

        public async Task<List<BankAccount>> GetByUserIdAsync(string userId)
        {

                     
            return await _context.BankAccounts
                .AsNoTracking()
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

        }
    }
}
