using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gosipjuice.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "4c49954e-4907-459d-8329-91a5cdded0d7";
            var writerRoleId = "edc615dd-5429-430e-a4eb-b36a4b8c95b4";
            // Create Reader and Writer Role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Create an Admin User
            var adminUserId = "d957472e-7e9c-499e-8111-46dafd43f18c";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@gosipjuice.com",
                Email = "admin@gosipjuice.com",
                NormalizedEmail = "admin@gosipjuice.com".ToUpper(),
                NormalizedUserName = "admin@gosipjuice.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);


            //Give Roles To Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = writerRoleId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

        }
    }
}
