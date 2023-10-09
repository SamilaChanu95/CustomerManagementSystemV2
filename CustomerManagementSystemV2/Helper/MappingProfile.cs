using AutoMapper;
using CustomerManagementSystemV2.Dtos;
using CustomerManagementSystemV2.Models;

namespace CustomerManagementSystemV2.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
        }

    }
}
