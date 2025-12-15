using Microsoft.EntityFrameworkCore;
using PersonalBlog3.Models;
using Microsoft.Extensions.Configuration;

namespace PersonalBlog3.Data
{
    public class BlogDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        // 只保留这一个构造函数（依赖注入+IConfiguration）
        public BlogDbContext(DbContextOptions<BlogDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_configuration.GetConnectionString("BlogDbContext"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "娱乐" },
                new Category { Id = 2, Name = "体育" },
                new Category { Id = 3, Name = "qq飞车游戏讨论" }
            );
        }
    }
}