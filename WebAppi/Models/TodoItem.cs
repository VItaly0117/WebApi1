using System.ComponentModel.DataAnnotations;

namespace WebAppi.Models;

public class TodoItem
{
    public int Id { get; set; }
    [Required]
    [MaxLength(200)]
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public Priority Priority { get; set; }
    public int TodoListId { get; set; }
    public TodoList TodoList { get; set; }
}
