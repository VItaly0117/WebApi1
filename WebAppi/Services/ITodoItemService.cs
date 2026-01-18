using WebAppi.Models;

namespace WebAppi.Services;

public interface ITodoItemService
{
    Task<OperationResult<IEnumerable<TodoItem>>> GetTodoItemsAsync(int todoListId, int userId);
    Task<OperationResult<TodoItem>> GetTodoItemAsync(int id, int userId);
    Task<OperationResult<TodoItem>> CreateTodoItemAsync(TodoItem todoItem, int userId);
    Task<OperationResult<TodoItem>> UpdateTodoItemAsync(int id, TodoItem todoItem, int userId);
    Task<OperationResult<bool>> DeleteTodoItemAsync(int id, int userId);
    Task<OperationResult<IEnumerable<TodoItem>>> SearchTodoItemsAsync(int userId, string title, bool? isCompleted, Priority? priority);
}
