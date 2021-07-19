using Microsoft.EntityFrameworkCore;

namespace EFConcurrencyDemo
{
    public class StudentContext : DbContext
    {
        public DbSet<StudentModel>? Students { get; set; }

        public StudentContext(DbContextOptions<StudentContext> options)
            : base(options)
        {
        }
    }
}
