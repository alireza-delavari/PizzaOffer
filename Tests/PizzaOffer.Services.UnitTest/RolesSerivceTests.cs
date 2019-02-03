using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using PizzaOffer.DataLayer.Context;
using PizzaOffer.Common.Utilities;
using Xunit;
using PizzaOffer.DomainClasses;
using System.Linq;

namespace PizzaOffer.Services.UnitTest
{
    public class RolesSerivceTests
    {
        private readonly IServiceProvider _serviceProvider;

        public RolesSerivceTests()
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("PizzaOfferInMemory1"));

            services.AddScoped<IUnitOfWork, ApplicationDbContext>();
            services.AddScoped<IRolesService, RolesService>();

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void FindUserRolesAsync_Default_OK()
        {
            // Insert seed data into the database using one instance of the context
            //using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    using (var context = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>())
            //    {
            //        context.Set<User>().Add(new User { Username = "username1", Password = "password1" });
            //        context.Set<User>().Add(new User { Username = "username2", Password = "password2" });
            //        context.Set<User>().Add(new User { Username = "username3", Password = "password3" });
            //        context.Set<Role>().Add(new Role { Name = "Admin" });
            //        context.Set<Role>().Add(new Role { Name = "Support" });
            //        context.Set<Role>().Add(new Role { Name = "User" });
            //        context.Set<UserRole>().Add(new UserRole { UserId = 1, RoleId = 1 });
            //        context.Set<UserRole>().Add(new UserRole { UserId = 1, RoleId = 2 });
            //        context.Set<UserRole>().Add(new UserRole { UserId = 1, RoleId = 3 });
            //        context.SaveChanges();
            //    }
            //}
            _serviceProvider.RunScopedService<IUnitOfWork>((context) =>
            {
                using (context)
                {
                    context.Set<User>().AddRange(
                        new User { Username = "username1", Password = "password1" },
                        new User { Username = "username2", Password = "password2" },
                        new User { Username = "username3", Password = "password3" });

                    context.Set<Role>().AddRange(
                        new Role { Name = "Admin" },
                        new Role { Name = "Support" },
                        new Role { Name = "User" });

                    context.Set<UserRole>().AddRange(
                        new UserRole { UserId = 1, RoleId = 1 },
                        new UserRole { UserId = 1, RoleId = 2 },
                        new UserRole { UserId = 1, RoleId = 3 });

                    context.SaveChanges();
                }
            });

            // Use a separate instance of the context to verify correct data was saved to database
            //using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    using (var context = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>())
            //    {
            //        Assert.Equal(3, context.Set<Role>().Count());
            //        Assert.Equal("Admin", context.Set<Role>().FirstOrDefault(q => q.Id == 1).Name);
            //    }
            //}
            _serviceProvider.RunScopedService<IUnitOfWork>((context) =>
            {
                using (context)
                {
                    Assert.Equal(3, context.Set<Role>().Count());
                    Assert.Equal("Admin", context.Set<Role>().FirstOrDefault(q => q.Id == 1).Name);
                }
            });


            // Use a clean instance of the context to run the test
            //using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            //{
            //    var rolesService = serviceScope.ServiceProvider.GetRequiredService<IRolesService>();
            //    var results = await rolesService.FindUserRolesAsync(1);
            //    Assert.Equal(3, results.Count);
            //}
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                var results = await roleService.FindUserRolesAsync(1);
                Assert.Equal(3, results.Count);
                Assert.Equal("Admin", results.FirstOrDefault(q => q.Id == 1).Name);
                Assert.Equal("Support", results.FirstOrDefault(q => q.Id == 2).Name);
                Assert.Equal("User", results.FirstOrDefault(q => q.Id == 3).Name);
            });

        }

    }
}
