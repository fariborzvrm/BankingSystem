using BankingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Interfaces
{
    public interface IBankAccountService
    {
        Task<BankAccountDto> CreateAccountAsync(string userId, string branchName, CancellationToken cancellationToken);
        Task<List<BankAccountDto>> GetUserAccountsAsync(string userId);

    }
}
