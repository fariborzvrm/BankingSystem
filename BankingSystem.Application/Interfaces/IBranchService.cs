using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Interfaces
{
    public interface IBranchService
    {
        Task<IReadOnlyList<string>> GetBranchesAsync(CancellationToken cancellationToken = default);
        Task<bool> IsValidBranchAsync(string branchName, CancellationToken cancellationToken = default);
    }
}
