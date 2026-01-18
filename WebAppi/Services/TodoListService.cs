using WebAppi.Models;
using WebAppi.Repositories;

namespace WebAppi.Services;

public class TodoListService(IUnitOfWork unitOfWork) : ITodoListService
{
    public async Task<OperationResult<IEnumerable<TodoList>>> GetTodoListsAsync(int userId)
    {
        var todoLists = await unitOfWork.TodoLists.FindAsync(tl => tl.UserId == userId);
        return OperationResult<IEnumerable<TodoList>>.SuccessResult(todoLists);
    }

    public async Task<OperationResult<TodoList>> GetTodoListAsync(int id, int userId)
    {
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(id);
        if (todoList == null || todoList.UserId != userId)
        {
            return OperationResult<TodoList>.FailureResult("Todo list not found.");
        }
        return OperationResult<TodoList>.SuccessResult(todoList);
    }

    public async Task<OperationResult<TodoList>> CreateTodoListAsync(TodoList todoList, int userId)
    {
        todoList.UserId = userId;
        await unitOfWork.TodoLists.AddAsync(todoList);
        await unitOfWork.CompleteAsync();
        return OperationResult<TodoList>.SuccessResult(todoList);
    }

    public async Task<OperationResult<TodoList>> UpdateTodoListAsync(int id, TodoList todoList, int userId)
    {
        var existingTodoList = await unitOfWork.TodoLists.GetByIdAsync(id);
        if (existingTodoList == null || existingTodoList.UserId != userId)
        {
            return OperationResult<TodoList>.FailureResult("Todo list not found.");
        }

        existingTodoList.Title = todoList.Title;
        unitOfWork.TodoLists.Update(existingTodoList);
        await unitOfWork.CompleteAsync();
        return OperationResult<TodoList>.SuccessResult(existingTodoList);
    }

    public async Task<OperationResult<bool>> DeleteTodoListAsync(int id, int userId)
    {
        var todoList = await unitOfWork.TodoLists.GetByIdAsync(id);
        if (todoList == null || todoList.UserId != userId)
        {
            return OperationResult<bool>.FailureResult("Todo list not found.");
        }

        unitOfWork.TodoLists.Remove(todoList);
        await unitOfWork.CompleteAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}
