using Microsoft.EntityFrameworkCore;
using mvcBasicCRUD.Models;

namespace mvcBasicCRUD.Data
{
    public class ProjectDBContext : DbContext
    {
        public DbSet<Chore>? Chores { get; set; }

        public DbSet<ChoreType>? ChoreTypes { get; set; }

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options) : base(options) { }
    }
}
