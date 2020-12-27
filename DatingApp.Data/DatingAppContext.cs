using Microsoft.EntityFrameworkCore;
using System;

namespace DatingApp.Data
{
    public partial class DatingAppContext : DbContext
    {
        public DatingAppContext(DbContextOptions<DatingAppContext> options): base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
    }
}
