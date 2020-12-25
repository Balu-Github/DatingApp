using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class DataContext : DbContext
    {
        protected DataContext(DbContextOptions<DbContext> options): base(options) { }

        public DbSet<Value> Values {get; set;}
    }
}