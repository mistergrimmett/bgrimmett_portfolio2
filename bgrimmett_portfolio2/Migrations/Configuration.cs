namespace bgrimmett_portfolio2.Migrations
{
    using bgrimmett_portfolio2.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<bgrimmett_portfolio2.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(bgrimmett_portfolio2.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Moderator"))
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }



            var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "mistergrimmett1127@gmail.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "mistergrimmett1127@gmail.com",
                    DisplayName = "Brandon Grimmett",
                    Email = "mistergrimmett1127@gmail.com",
                    FirstName = "Brandon",
                    LastName = "Grimmett",
                }, "Powerman@1");
            }

            if (!context.Users.Any(u => u.Email == "ewatkins@coderfoundry.com"))
            {
                userManager.Create(new ApplicationUser         //Creating new user for the application using required fields
                {
                    UserName = "ewatkins@coderfoundry.com",
                    DisplayName = "Eric Watkins",
                    Email = "ewatkins@coderfoundry.com",
                    FirstName = "Eric",
                    LastName = "Watkins",
                }, "abc@123");
            }



            var adminId = userManager.FindByEmail("mistergrimmett1127@gmail.com").Id;
            userManager.AddToRole(adminId, "Admin");

            var moderatorId = userManager.FindByEmail("ewatkins@coderfoundry.com").Id;
            userManager.AddToRole(moderatorId, "Moderator");




            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
