using AutoMapper;
using System.Diagnostics.Contracts;

namespace InventoryManagement.Common.Mapping
{
    public class Mapping : Profile
    {
        /// <summary>
        /// Mappings for data model and customer facing contracts
        /// </summary>
        public Mapping()
        {
            CreateMap<Contracts.Customer, EntityFrameworkCore.Customer>().ReverseMap(); //needed as of now 
          //  CreateMap<EntityFrameworkCore.Customer, Contracts.Customer>();
            CreateMap<Contracts.Product, EntityFrameworkCore.Product>().ReverseMap(); //needed as of now 
         //   CreateMap<EntityFrameworkCore.Product, Contracts.Product>();
            CreateMap<EntityFrameworkCore.Order, Contracts.OrderResponse>().ReverseMap();
         //   CreateMap<Contracts.OrderResponse, EntityFrameworkCore.Order>();

            /*ISourceMemberConfigurationExpression.Ignore was renamed to DoNotValidate to avoid confusion.
                It only applies when validating source members, with MemberList.Source.
                It does not affect the mapping itself or validation for the default case,
            MemberList.Destination.To migrate, replace all usages of Ignore with DoNotValidate:
          Eg:  cfg.CreateMap<Source, Dest>()
                .ForSourceMember(source => source.Date, opt => opt.DoNotValidate());*/
        }

    }
}