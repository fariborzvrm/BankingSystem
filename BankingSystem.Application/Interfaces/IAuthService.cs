using BankingSystem.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Interfaces
{
    public interface IAuthService
    {
        Task <AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
