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
            CreateMap<Contracts.Customer, EntityFrameworkCore.Customer>(); //needed as of now 
            CreateMap<EntityFrameworkCore.Customer, Contracts.Customer>();
            CreateMap<Contracts.Product, EntityFrameworkCore.Product>(); //needed as of now 
            CreateMap<EntityFrameworkCore.Product, Contracts.Product>();
        }

    }
}