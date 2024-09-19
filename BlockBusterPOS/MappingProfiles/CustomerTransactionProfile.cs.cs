using AutoMapper;
using BlockBusterPOS.Dto;
using BlockBusterPOS.Models;

namespace BlockBusterPOS.MappingProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<CustomerTransactionModel, CustomerTransactionDto>();

        CreateMap<CustomerModel, CustomerModelDto>();

        CreateMap<RentalModel, RentalModelDto>();

        CreateMap<CreateCustomerTransactionDto, CustomerTransactionModel>()
            .ForMember(dest => dest.TotalRentalPrice, opt => opt.Ignore());

        CreateMap<RentalModelDtoBasic, RentalModel>()
            .ForMember(dest => dest.RentalDate, opt => opt.Ignore());
    }
}
