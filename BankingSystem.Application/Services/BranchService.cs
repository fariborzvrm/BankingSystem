using BankingSystem.Application.Interfaces;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace BankingSystem.Application.Services
{
    public class BranchService : IBranchService
    {
        private const string CacheKey = "branches-list";
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public BranchService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<IReadOnlyList<string>> GetBranchesAsync(CancellationToken cancellationToken = default)
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<string> cachedBranches))
            return cachedBranches;

            var branches = await _httpClient.GetFromJsonAsync<List<string>>("api/branches", cancellationToken)
                      ?? new List<string>();

            _cache.Set(CacheKey, branches, TimeSpan.FromMinutes(1));

            return branches;
        }

        public async Task<bool> IsValidBranchAsync(string branchName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(branchName))
                return false;

            var branches = await GetBranchesAsync(cancellationToken);

            return branches.Any(b =>
                string.Equals(b.Trim(), branchName.Trim(), StringComparison.OrdinalIgnoreCase));
        }

    }
}
