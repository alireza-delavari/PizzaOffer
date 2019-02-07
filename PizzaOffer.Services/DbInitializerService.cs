using Microsoft.Extensions.DependencyInjection;
using PizzaOffer.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PizzaOffer.Common;
using PizzaOffer.DomainClasses;

namespace PizzaOffer.Services
{
    public interface IDbInitializerService
    {
        /// <summary>
        /// Applies any pending migrations for the context to the database.
        /// Will create the database if it does not already exist.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Adds some default values to the Db
        /// </summary>
        void SeedData();
    }

    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISecurityService _securityService;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory,
            ISecurityService securityService)
        {
            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public async void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    // add seed data here
                    var adminRole = new Role { Name = CustomRoles.Admin };
                    var userRole = new Role { Name = CustomRoles.User };
                    var EditorRole = new Role { Name = CustomRoles.Editor };
                    if (!await context.Set<Role>().AnyAsync())
                    {
                        context.Add(adminRole);
                        context.Add(userRole);
                        context.Add(EditorRole);
                        context.SaveChanges();
                    }

                    // Add Admin user
                    //todo: add from appSetting.json (appSetting.Production.json)
                    if (!await context.Set<User>().AnyAsync(q => q.Username == "Alireza"))
                    {
                        var adminUser = new User
                        {
                            Username = "Alireza",
                            DisplayName = "علیرضا",
                            IsUserActive = true,
                            LastVisitDate = null,
                            //todo: !important get password from appSetting.json (appSetting.Production.json)
                            Password = _securityService.GetSha256Hash("alirezaisadmin321654987"),
                            SerialNumber = Guid.NewGuid().ToString("N"),
                            Email = "test@gmail.com",
                            IsEmailConfirmed = true,
                            PhoneNumber = "09116662266",
                            IsPhoneConfirmed = true
                        };
                        context.Add(adminUser);
                        context.SaveChanges();

                        context.Add(new UserRole { Role = await context.Set<Role>().FirstOrDefaultAsync(x => x.Name == CustomRoles.Admin), User = adminUser });
                        context.Add(new UserRole { Role = await context.Set<Role>().FirstOrDefaultAsync(x => x.Name == CustomRoles.Editor), User = adminUser });
                        context.Add(new UserRole { Role = await context.Set<Role>().FirstOrDefaultAsync(x => x.Name == CustomRoles.User), User = adminUser });

                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
