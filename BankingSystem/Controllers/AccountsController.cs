using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace BankingSystem.Api.Controllers
{
    [Authorize]    
    [ApiController]
    [Route("api/accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public AccountsController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        [HttpPost]
        public async Task<ActionResult<BankAccountDto>> CreateAccount([FromQuery] string branchName, CancellationToken cancellationToken)
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var createdAccount = await _bankAccountService.CreateAccountAsync(userId, branchName, cancellationToken);


            return Ok(createdAccount);
        }

        [HttpGet]
        public async Task<ActionResult<List<BankAccountDto>>> GetUserAccounts()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be found in the token.");
            }

            var accounts = await _bankAccountService.GetUserAccountsAsync(userId);
            return Ok(accounts);
        }
    }
}
