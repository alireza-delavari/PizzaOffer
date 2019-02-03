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
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                var results = await roleService.FindUserRolesAsync(1);
                Assert.Equal(2, results.Count);
                Assert.Equal("Admin", results.FirstOrDefault(q => q.Id == 1).Name);
                Assert.Equal("Support", results.FirstOrDefault(q => q.Id == 2).Name);
                var results2 = await roleService.FindUserRolesAsync(4);
                Assert.Empty(results2);
            });
        }

        [Fact]
        public void FindUserRolesAsync_WhenUserDoesNotExists_ReturnsEmptyList()
        {
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                var results = await roleService.FindUserRolesAsync(500);
                Assert.NotNull(results);
                Assert.Empty(results);
            });
        }

        [Fact]
        public void FindUsersInRoleAsync_Default_OK()
        {
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                var results = await roleService.FindUsersInRoleAsync("Support");
                Assert.Equal(3, results.Count);
                var results2 = await roleService.FindUsersInRoleAsync("User2");
                Assert.Empty(results2);
            });
        }

        [Fact]
        public void FindUsersInRoleAsync_WhenRoleNameDoesNotExists_ReturnsEmptyList()
        {
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                var results = await roleService.FindUsersInRoleAsync("RoleNotExists");
                Assert.NotNull(results);
                Assert.Empty(results);
            });
        }

        [Fact]
        public void IsUserInRoleAsync_Default_OK()
        {
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                Assert.True(await roleService.IsUserInRoleAsync(1, "Admin"));
                Assert.True(await roleService.IsUserInRoleAsync(1, "Support"));
                Assert.False(await roleService.IsUserInRoleAsync(1, "User2"));
                Assert.False(await roleService.IsUserInRoleAsync(2, "Admin"));
                Assert.False(await roleService.IsUserInRoleAsync(4, "Admin"));
            });
        }

        [Fact]
        public void IsUserInRoleAsync_WhenUserIdOrRoleNameDoesNotExists_ReturnsFalse()
        {
            InsertSeedData();
            TestSeedData();

            // Use a clean instance of the context to run the test
            _serviceProvider.RunScopedService<IRolesService>(async (roleService) =>
            {
                Assert.False(await roleService.IsUserInRoleAsync(100, "Admin"));
                Assert.False(await roleService.IsUserInRoleAsync(1, "RoleNotExists"));
                Assert.False(await roleService.IsUserInRoleAsync(100, "RoleNotExists"));
            });
        }



        private void InsertSeedData()
        {
            // Insert seed data into the database using one instance of the context
            _serviceProvider.RunScopedService<IUnitOfWork>((context) =>
            {
                using (context)
                {
                    context.Set<User>().AddRange(
                        new User { Username = "username1", Password = "password1" },
                        new User { Username = "username2", Password = "password2" },
                        new User { Username = "username3", Password = "password3" },
                        new User { Username = "username4", Password = "password4" });

                    context.Set<Role>().AddRange(
                        new Role { Name = "Admin" },
                        new Role { Name = "Support" },
                        new Role { Name = "User" },
                        new Role { Name = "User2" });

                    context.Set<UserRole>().AddRange(
                        new UserRole { UserId = 1, RoleId = 1 },
                        new UserRole { UserId = 1, RoleId = 2 },
                        new UserRole { UserId = 2, RoleId = 2 },
                        new UserRole { UserId = 3, RoleId = 2 });

                    context.SaveChanges();
                }
            });
        }

        private void TestSeedData()
        {
            // Use a separate instance of the context to verify correct data was saved to database
            _serviceProvider.RunScopedService<IUnitOfWork>((context) =>
            {
                using (context)
                {
                    Assert.Equal(4, context.Set<Role>().Count());
                    Assert.Equal(4, context.Set<User>().Count());
                    Assert.Equal(4, context.Set<UserRole>().Count());
                    Assert.Equal("username1", context.Set<User>().FirstOrDefault(q => q.Id == 1).Username);
                    Assert.Equal("Admin", context.Set<Role>().FirstOrDefault(q => q.Id == 1).Name);
                }
            });
        }
    }
}
