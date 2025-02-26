using Microsoft.EntityFrameworkCore;

namespace DynamicForm.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Form> DynamicForms { get; set; }
        public DbSet<FormDetail> FormDetails { get; set; }
    }
}
