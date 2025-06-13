using static System.Runtime.InteropServices.JavaScript.JSType;
using WarehouseApplication.Dtos;
using WarehouseApplication.Models;
using AutoMapper;

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
