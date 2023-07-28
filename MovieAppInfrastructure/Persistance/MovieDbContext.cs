using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieAppDomain.Entities;

namespace MovieAppInfrastructure.Persistance
{
    public class MovieDbContext : IdentityDbContext
    {
        public MovieDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Movies> Movies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
