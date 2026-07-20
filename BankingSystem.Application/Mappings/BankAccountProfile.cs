using AutoMapper;
using BankingSystem.Application.DTOs;
using BankingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem.Application.Mappings
{
    public class BankAccountProfile : Profile
    {
        public BankAccountProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
            
        }
    }
}
