using BookIt.Core.Entities;
using BookIt.DataAccess.Persistence;

namespace BookIt.DataAccess.Repositories.Impl;

public class TodoItemRepository : BaseRepository<TodoItem>, ITodoItemRepository
{
    public TodoItemRepository(DatabaseContext context) : base(context) { }
}
