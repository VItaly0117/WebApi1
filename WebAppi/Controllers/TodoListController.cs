using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppi.Models;
using WebAppi.Services;

namespace WebAppi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TodoListController(ITodoListService todoListService) : ControllerBase
{
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    [HttpGet]
    public async Task<ActionResult<OperationResult<IEnumerable<TodoList>>>> GetTodoLists()
    {
        var result = await todoListService.GetTodoListsAsync(UserId);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OperationResult<TodoList>>> GetTodoList(int id)
    {
        var result = await todoListService.GetTodoListAsync(id, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OperationResult<TodoList>>> CreateTodoList(TodoList todoList)
    {
        var result = await todoListService.CreateTodoListAsync(todoList, UserId);
        return CreatedAtAction(nameof(GetTodoList), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodoList(int id, TodoList todoList)
    {
        var result = await todoListService.UpdateTodoListAsync(id, todoList, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoList(int id)
    {
        var result = await todoListService.DeleteTodoListAsync(id, UserId);
        if (!result.Success)
        {
            return NotFound(result);
        }
        return NoContent();
    }
}
