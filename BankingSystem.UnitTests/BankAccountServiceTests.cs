using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using BankingSystem.Application.Services;
using BankingSystem.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace BankingSystem.UnitTests
{
    public class BankAccountServiceTests
    {
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBranchService> _branchServiceMock;

        private readonly BankAccountService _service;

        public BankAccountServiceTests()
        {
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _branchServiceMock = new Mock<IBranchService>();

            _service = new BankAccountService(
                _bankAccountRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _branchServiceMock.Object);
        }

        // UnitTests for CreateAccountAsync method

        [Fact]
        public async Task CreateAccountAsync_WhenUserIdIsEmpty_ShouldThrowArgumentException()
        {
            
            string userId = "";
            string branchName = "TestBranch";
            
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAccountAsync(userId, branchName, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAccountAsync_WhenBranchNameIsEmpty_ShouldThrowArgumentException()
        {
            string userId = "User-1";
            string branchName = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAccountAsync(userId,branchName, CancellationToken.None));
        }



        [Fact]
        public async Task CreateAccountAsync_WhenBranchIsInvalid_ShouldThrowInvalidOperationException()
        {
            string userId = "test-user-id";
            string branchName = "InvalidBranch";

            _branchServiceMock.Setup(b => b.IsValidBranchAsync(branchName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAccountAsync(userId, branchName, CancellationToken.None));
        }

        [Fact]
        public async Task CreateAccountAsync_WhenInputIsValid_ShouldCreateAccountAndReturnDto()
        {
            string userId = "test-user-id";
            string branchName = "ValidBranch";

            _branchServiceMock.Setup(b => b.IsValidBranchAsync(branchName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _bankAccountRepositoryMock
                .Setup(x => x.ExistsByAccountNumberAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var expectedDto = new BankAccountDto
            {
                Id = Guid.NewGuid(),
                AccountNumber = "1234567890",
                Balance = 100,
                CreatedAt = DateTime.UtcNow
            };

            _mapperMock.Setup(x => x.Map<BankAccountDto>(It.IsAny<object>()))
                .Returns(expectedDto);

            var result = await _service.CreateAccountAsync(userId, branchName, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.AccountNumber, result.AccountNumber);
            Assert.Equal(expectedDto.Balance, result.Balance);

            _bankAccountRepositoryMock.Verify(b => b.AddAsync(It.IsAny<BankAccount>()), Times.Once);
            _unitOfWorkMock.Verify(b => b.SaveChangesAsync(), Times.Once);

        }


        // UnitTests for GetUserAccountsAsync method

        [Fact]
        public async Task GetUserAccountsAsync_WhenUserIdIsValid_ShouldReturnMappedAccounts()
        {
            string userId = "User-1";

            var accounts = new List<BankAccount>
            {
                new BankAccount { UserId = userId,Balance = 100 },
                new BankAccount { UserId = userId, Balance = 200 }
            };

            var expectedDtos = new List<BankAccountDto> {   
                
            new BankAccountDto(),
            new BankAccountDto()

            };

            _bankAccountRepositoryMock.Setup(x => x.GetByUserIdAsync(userId))
                .ReturnsAsync(accounts);

            _mapperMock.Setup(x => x.Map<List<BankAccountDto>>(accounts))
            .Returns(expectedDtos);

            var result = await _service.GetUserAccountsAsync(userId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUserAccountsAsync_WhenUserIdIsEmpty_ShouldThrowArgumentException()
        {
            string userId = "";

            await Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetUserAccountsAsync(userId));
           
        }


    }
}
