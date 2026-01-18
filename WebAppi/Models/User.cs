using Microsoft.AspNetCore.Identity;

namespace WebAppi.Models;

public class User : IdentityUser<int>
{
    public ICollection<TodoList> TodoLists { get; set; }
}
