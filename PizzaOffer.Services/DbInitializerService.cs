using Microsoft.Extensions.DependencyInjection;
using PizzaOffer.DataLayer.Context;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using PizzaOffer.Common;
using PizzaOffer.DomainClasses;
using Microsoft.Extensions.Options;
using PizzaOffer.Services.Options;
using System.Threading.Tasks;

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
        Task<(bool Succeeded, string Error)> SeedDatabaseAdminUserAsync();
    }

    public class DbInitializerService : IDbInitializerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ISecurityService _securityService;
        private readonly IOptions<AdminUserSeedOptions> _adminUserSeedOptions;
        private readonly IUsersService _usersService;
        private readonly IRolesService _rolesService;

        public DbInitializerService(
            IServiceScopeFactory scopeFactory,
            ISecurityService securityService,
            IOptions<AdminUserSeedOptions> adminUserSeedOptions,
            IUsersService usersService,
            IRolesService rolesService
            )
        {
            _scopeFactory = scopeFactory;
            _scopeFactory.CheckArgumentIsNull(nameof(_scopeFactory));

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _adminUserSeedOptions = adminUserSeedOptions;
            _adminUserSeedOptions.CheckArgumentIsNull(nameof(_adminUserSeedOptions));

            _usersService = usersService;
            _usersService.CheckArgumentIsNull(nameof(_usersService));

            _rolesService = rolesService;
            _rolesService.CheckArgumentIsNull(nameof(_rolesService));
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

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {

                var dbInitializerService = serviceScope.ServiceProvider.GetService<IDbInitializerService>();
                var result = dbInitializerService.SeedDatabaseAdminUserAsync().Result;
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(result.Error);
                }
            }
        }

        public async Task<(bool Succeeded, string Error)> SeedDatabaseAdminUserAsync()
        {
            var adminUserSeed = _adminUserSeedOptions.Value;
            adminUserSeed.CheckArgumentIsNull(nameof(adminUserSeed));

            var username = adminUserSeed.Username;
            var password = adminUserSeed.Password;
            var email = adminUserSeed.Email;
            var roleName = adminUserSeed.RoleName;
            var phoneNumber = adminUserSeed.PhoneNumber;
            var displayName = adminUserSeed.DisplayName;

            var adminUser = await _usersService.FindUserAsync(username);
            if (adminUser != null)
            {
                //_logger.LogInformation($"{thisMethodName}: adminUser already exists.");
                return (true, string.Empty);
            }

            //Create the `Admin` Role if it does not exist
            var adminRole = await _rolesService.FindRoleAsync(roleName);
            if (adminRole == null)
            {
                adminRole = new Role() { Name = roleName };
                var adminRoleResult = await _rolesService.CreateRoleAsync(adminRole);
                if (!adminRoleResult.Succeeded)
                {
                    //_logger.LogError($"{thisMethodName}: adminRole CreateAsync failed. {adminRoleResult.DumpErrors()}");
                    return (false, adminRoleResult.Error);
                }
            }
            else
            {
                //_logger.LogInformation($"{thisMethodName}: adminRole already exists.");
            }

            adminUser = new User
            {
                Username = username,
                Email = email,
                DisplayName = displayName,
                PhoneNumber = phoneNumber,
                IsEmailConfirmed = true,
                IsPhoneConfirmed = true,
                IsUserActive = true,
            };

            var adminUserResult = await _usersService.CreateUserAsync(adminUser, password);
            if (!adminUserResult.Succeeded)
            {
                //_logger.LogError($"{thisMethodName}: adminUser CreateAsync failed. {adminUserResult.DumpErrors()}");
                return (false, adminUserResult.Error);
            }

            //var setLockoutResult = await _applicationUserManager.SetLockoutEnabledAsync(adminUser, enabled: false);
            //if (setLockoutResult == IdentityResult.Failed())
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser SetLockoutEnabledAsync failed. {setLockoutResult.DumpErrors()}");
            //    return IdentityResult.Failed();
            //}

            await _rolesService.AddUserInRoleAsync(adminUser, adminRole);
            //var addToRoleResult = await _rolesService.AddUserInRoleAsync(adminUser,adminRole);//.AddToRoleAsync(adminUser, adminRole.Name);
            //if (addToRoleResult == IdentityResult.Failed())
            //{
            //    _logger.LogError($"{thisMethodName}: adminUser AddToRoleAsync failed. {addToRoleResult.DumpErrors()}");
            //    return IdentityResult.Failed();
            //}

            return (true, string.Empty);
        }

    }
}
