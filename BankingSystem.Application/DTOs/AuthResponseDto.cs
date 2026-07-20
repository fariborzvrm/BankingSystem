using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.DTOs
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
