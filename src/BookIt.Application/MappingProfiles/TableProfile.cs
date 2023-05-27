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
            CreateMap<Table, TableModelForCreate>();
            CreateMap<TableModelForUpdate, Table>();
            CreateMap<Table, TableModelForUpdate>();
            CreateMap<TableModel, Table>();
            CreateMap<Table, TableModel>();
        }
    }
}
