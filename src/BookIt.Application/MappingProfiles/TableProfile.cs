using AutoMapper;
using BookIt.Application.Models.Table;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles
{
    public class TableProfile : Profile
    {
        public TableProfile()
        {
            CreateMap<TableModelForCreate, Table>();
            CreateMap<Restaurant, TableModelForCreate>();
            CreateMap<TableModel, Table>();
            CreateMap<Table, TableModel>();
        }
    }
}
