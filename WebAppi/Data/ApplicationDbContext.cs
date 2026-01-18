using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAppi.Models;

namespace WebAppi.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
{
    public DbSet<TodoList> TodoLists { get; set; }
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(u => u.TodoLists)
            .WithOne(tl => tl.User)
            .HasForeignKey(tl => tl.UserId);

        builder.Entity<TodoList>()
            .HasMany(tl => tl.TodoItems)
            .WithOne(ti => ti.TodoList)
            .HasForeignKey(ti => ti.TodoListId);
    }
}
