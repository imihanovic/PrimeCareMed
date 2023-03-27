using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;

namespace BookIt.DataAccess.Repositories.Impl;

public class TodoListRepository : BaseRepository<TodoList>, ITodoListRepository
{
    public TodoListRepository(DatabaseContext context) : base(context) { }
}
