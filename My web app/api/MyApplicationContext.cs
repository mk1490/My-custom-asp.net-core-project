using Microsoft.EntityFrameworkCore;
using My_web_app.api.v1.Users.Admin.Users.Model;

namespace My_web_app.api
{
    public class MyApplicationContext : DbContext
    {
        public MyApplicationContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BaseUsersModel> UserModel { get; set; }

        public DbSet<BaseUserDetailsModel> UserDetailsModel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("matin_schema");
            base.OnModelCreating(modelBuilder);
        }
    }
}