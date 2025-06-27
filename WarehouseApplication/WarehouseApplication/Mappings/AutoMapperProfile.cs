using AutoMapper;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;

namespace WarehouseApplication.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Contractor, ContractorDto>().ReverseMap();
            CreateMap<Document, DocumentDto>().ReverseMap();
            CreateMap<DocumentItem, DocumentItemDto>().ReverseMap();
        }
    }
}
