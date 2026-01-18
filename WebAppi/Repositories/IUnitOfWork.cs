using WebAppi.Models;

namespace WebAppi.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<TodoList> TodoLists { get; }
    IRepository<TodoItem> TodoItems { get; }
    Task<int> CompleteAsync();
}
