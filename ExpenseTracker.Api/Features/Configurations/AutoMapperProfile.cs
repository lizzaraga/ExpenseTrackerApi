using AutoMapper;
using ExpenseTracker.Api.Features.Purses.Dtos;
using ExpenseTracker.Database.Entities;

namespace ExpenseTracker.Api.Features.Configurations;

public class AutoMapperProfile: Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreatePurseDto, Purse>();
    }
}