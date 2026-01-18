using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public IRepository<TodoList> TodoLists { get; } = new Repository<TodoList>(context);
    public IRepository<TodoItem> TodoItems { get; } = new Repository<TodoItem>(context);

    public async Task<int> CompleteAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
