using Microsoft.EntityFrameworkCore;
using WebAppi.Models;
using WebAppi.Repositories;

namespace WebAppi.Services;

public class TodoItemService(IUnitOfWork unitOfWork) : ITodoItemService
{
    public async Task<OperationResult<IEnumerable<TodoItem>>> GetTodoItemsAsync(int todoListId, int userId)
    {
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(todoListId);
        if (todoList == null || todoList.UserId != userId)
        {
            return OperationResult<IEnumerable<TodoItem>>.FailureResult("Todo list not found.");
        }
        var todoItems = await unitOfWork.TodoItems.FindAsync(ti => ti.TodoListId == todoListId);
        return OperationResult<IEnumerable<TodoItem>>.SuccessResult(todoItems);
    }

    public async Task<OperationResult<TodoItem>> GetTodoItemAsync(int id, int userId)
    {
        var todoItem = await unitOfWork.TodoItems.GetByIdAsync(id);
        if (todoItem == null)
        {
            return OperationResult<TodoItem>.FailureResult("Todo item not found.");
        }
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(todoItem.TodoListId);
        if (todoList.UserId != userId)
        {
            return OperationResult<TodoItem>.FailureResult("Access denied.");
        }
        return OperationResult<TodoItem>.SuccessResult(todoItem);
    }

    public async Task<OperationResult<TodoItem>> CreateTodoItemAsync(TodoItem todoItem, int userId)
    {
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(todoItem.TodoListId);
        if (todoList == null || todoList.UserId != userId)
        {
            return OperationResult<TodoItem>.FailureResult("Todo list not found.");
        }
        await unitOfWork.TodoItems.AddAsync(todoItem);
        await unitOfWork.CompleteAsync();
        return OperationResult<TodoItem>.SuccessResult(todoItem);
    }

    public async Task<OperationResult<TodoItem>> UpdateTodoItemAsync(int id, TodoItem todoItem, int userId)
    {
        var existingTodoItem = await unitOfWork.TodoItems.GetByIdAsync(id);
        if (existingTodoItem == null)
        {
            return OperationResult<TodoItem>.FailureResult("Todo item not found.");
        }
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(existingTodoItem.TodoListId);
        if (todoList.UserId != userId)
        {
            return OperationResult<TodoItem>.FailureResult("Access denied.");
        }

        existingTodoItem.Title = todoItem.Title;
        existingTodoItem.IsCompleted = todoItem.IsCompleted;
        existingTodoItem.Priority = todoItem.Priority;
        unitOfWork.TodoItems.Update(existingTodoItem);
        await unitOfWork.CompleteAsync();
        return OperationResult<TodoItem>.SuccessResult(existingTodoItem);
    }

    public async Task<OperationResult<bool>> DeleteTodoItemAsync(int id, int userId)
    {
        var todoItem = await unitOfWork.TodoItems.GetByIdAsync(id);
        if (todoItem == null)
        {
            return OperationResult<bool>.FailureResult("Todo item not found.");
        }
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(todoItem.TodoListId);
        if (todoList.UserId != userId)
        {
            return OperationResult<bool>.FailureResult("Access denied.");
        }

        unitOfWork.TodoItems.Remove(todoItem);
        await unitOfWork.CompleteAsync();
        return OperationResult<bool>.SuccessResult(true);
    }

    public async Task<OperationResult<IEnumerable<TodoItem>>> SearchTodoItemsAsync(int userId, string title, bool? isCompleted, Priority? priority)
    {
        var query = (await unitOfWork.TodoItems.FindAsync(ti => ti.TodoList.UserId == userId)).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(ti => ti.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        if (isCompleted.HasValue)
        {
            query = query.Where(ti => ti.IsCompleted == isCompleted.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(ti => ti.Priority == priority.Value);
        }

        return OperationResult<IEnumerable<TodoItem>>.SuccessResult(query.ToList());
    }
}
