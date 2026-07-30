using BankingSystem.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace BankingSystem.Infrastructure.Services
{
    public class BranchService : IBranchService
    {
        private const string CacheKey = "branches-list";
        private readonly HttpClient  _httpClinet;
        private readonly IMemoryCache _cache;

        public BranchService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClinet = httpClient;
            _cache = cache;
        }

        public async Task<IReadOnlyList<string>> GetBranchesAsync(CancellationToken cancellationToken = default)
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<string>? cachedBranches))
                return cachedBranches!;

            var branches = await _httpClinet
                .GetFromJsonAsync<List<string>>("api/branches", cancellationToken)
                ?? new List<string>();

            _cache.Set(CacheKey, branches, TimeSpan.FromMinutes(1));

            return branches;
        }

        public async Task<bool> IsValidBranchAsync(string branchName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(branchName))

                return false;


            var branches = await GetBranchesAsync(cancellationToken);


            var trimmedBranchName = branchName.Trim();

            return branches.Any(b =>
                string.Equals(b.Trim(), trimmedBranchName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
