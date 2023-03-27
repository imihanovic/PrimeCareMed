using AutoMapper;
using BookIt.Application.Models.TodoList;
using BookIt.Core.Entities;

namespace BookIt.Application.MappingProfiles;

public class TodoListProfile : Profile
{
    public TodoListProfile()
    {
        CreateMap<CreateTodoListModel, TodoList>();

        CreateMap<TodoList, TodoListResponseModel>();
    }
}
