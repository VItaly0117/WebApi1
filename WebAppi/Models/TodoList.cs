using System.ComponentModel.DataAnnotations;

namespace WebAppi.Models;

public class TodoList
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }
    public ICollection<TodoItem> TodoItems { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
