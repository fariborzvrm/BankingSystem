using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Services
{
    public class BankAccountService : IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IUnitOfWork unitOfWork, IMapper mapper, IBranchService branchService)
        {
            _bankAccountRepository = bankAccountRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _branchService = branchService;
        }

        // Generates a unique account number based on the current timestamp.
        private string GenerateAccountNumber()
        {
            return DateTime.UtcNow.Ticks.ToString();
        }

        public async Task<BankAccountDto> CreateAccountAsync(string userId, string branchName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrWhiteSpace(branchName))
                throw new ArgumentException("Branch name is required.", nameof(branchName));

            var isValidBranch = await _branchService.IsValidBranchAsync(branchName, cancellationToken);
            if (!isValidBranch)
            {
                
                throw new InvalidOperationException($"The branch '{branchName}' is not a valid bank branch.");
            }

            string accountNumber;

            do
            {
                accountNumber = GenerateAccountNumber();
            }
            while (await _bankAccountRepository.ExistsByAccountNumberAsync(accountNumber));

            var bankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountNumber = accountNumber,
                Balance = 0m,
                CreatedAt = DateTime.UtcNow
            };

            await _bankAccountRepository.AddAsync(bankAccount);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BankAccountDto>(bankAccount);


        }

        public async Task<List<BankAccountDto>> GetUserAccountsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            var bankAccounts = await _bankAccountRepository.GetByUserIdAsync(userId);

            return _mapper.Map<List<BankAccountDto>>(bankAccounts);
        }
    }
}