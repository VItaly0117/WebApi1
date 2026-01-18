using WebAppi.Models;

namespace WebAppi.Services;

public interface ITodoListService
{
    Task<OperationResult<IEnumerable<TodoList>>> GetTodoListsAsync(int userId);
    Task<OperationResult<TodoList>> GetTodoListAsync(int id, int userId);
    Task<OperationResult<TodoList>> CreateTodoListAsync(TodoList todoList, int userId);
    Task<OperationResult<TodoList>> UpdateTodoListAsync(int id, TodoList todoList, int userId);
    Task<OperationResult<bool>> DeleteTodoListAsync(int id, int userId);
}
