using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using portalBalance.Models;
using Microsoft.EntityFrameworkCore;

namespace portalBalance.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new PortalBalanceContext(
                serviceProvider.GetRequiredService<DbContextOptions<PortalBalanceContext>>()))
            {
                // Look for any SuperAdmin.
                if (context.SuperAdmins.Any())
                {
                    return;   // DB has been seeded
                }

                var superAdminHasher = new PasswordHasher<SuperAdmin>();
                var superAdmin = new SuperAdmin
                {
                    Name = "SuperAdmin",
                    Email = "superadmin@example.com",
                    Password = superAdminHasher.HashPassword(null, "SuperAdminPassword")
                };
                context.SuperAdmins.Add(superAdmin);

            

                context.SaveChanges();
            }
        }
    }
}
