using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Infrastructure.Identity
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequestDto.Email);

            if (existingUser != null)
            {

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = $"User creation failed: {errors}"
                };
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");

            if (!roleResult.Succeeded)
            {

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Failed to assign role to user."
                };
            }

            return new AuthResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                IsSuccess = true,
                Message = "User registered successfully."
            };

        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User does not exist."
                };
            }
                var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (!isPasswordValid)
                {
                    return new AuthResponseDto
                    {
                        IsSuccess = false,
                        Message = "Invalid username or password."
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: expires,
                    signingCredentials: creds
                );

                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenString = tokenHandler.WriteToken(token);

                return new AuthResponseDto
                {
                    UserId = user.Id,
                    Email = user.Email ?? string.Empty,
                    IsSuccess = true,
                    Message = "Login successful.",
                    Token = tokenString
                };

            }
        }
    }

