using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppi.Models;
using WebAppi.Services;

namespace WebAppi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoItemController(ITodoItemService todoItemService) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet("list/{todoListId}")]
    public async Task<ActionResult<OperationResult<IEnumerable<TodoItem>>>> GetTodoItems(int todoListId)
    {
        var result = await todoItemService.GetTodoItemsAsync(todoListId, UserId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult<TodoItem>>> GetTodoItem(int id)
    {
        var result = await todoItemService.GetTodoItemAsync(id, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OperationResult<TodoItem>>> CreateTodoItem(TodoItem todoItem)
    {
        var result = await todoItemService.CreateTodoItemAsync(todoItem, UserId);
        if (!result.Success)
        {
            return BadRequest(result);
        }
        return CreatedAtAction(nameof(GetTodoItem), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoItem(int id, TodoItem todoItem)
    {
        var result = await todoItemService.UpdateTodoItemAsync(id, todoItem, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(int id)
    {
        var result = await todoItemService.DeleteTodoItemAsync(id, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<OperationResult<IEnumerable<TodoItem>>>> SearchTodoItems([FromQuery] string title, [FromQuery] bool? isCompleted, [FromQuery] Priority? priority)
    {
        var result = await todoItemService.SearchTodoItemsAsync(UserId, title, isCompleted, priority);
        return Ok(result);
    }
}
