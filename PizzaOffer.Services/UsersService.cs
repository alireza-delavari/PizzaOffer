using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PizzaOffer.Common;
using PizzaOffer.DataLayer.Context;
using PizzaOffer.DomainClasses;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PizzaOffer.Services
{
    public interface IUsersService
    {
        Task<string> GetSerialNumberAsync(int userId);
        Task<User> FindUserAsync(string username, string password);
        Task<User> FindUserAsync(int userId);
        Task<User> FindUserAsync(string username);
        Task<List<User>> GetAllUsersAsync();
        Task UpdateUserLastActivityDateAsync(int userId);
        Task<User> GetCurrentUserAsync();
        string GetCurrentUserDisplayName();
        string GetCurrentUserUsername();
        int GetCurrentUserId();
        Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        Task<(bool Succeeded, string Error)> CreateUserAsync(User user, string password);
    }

    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<User> _users;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRolesService _roleService;

        public UsersService(
            IUnitOfWork uow,
            ISecurityService securityService,   
            IHttpContextAccessor contextAccessor,
            IRolesService roleService)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _users = _uow.Set<User>();

            _securityService = securityService;
            _securityService.CheckArgumentIsNull(nameof(_securityService));

            _contextAccessor = contextAccessor;
            _contextAccessor.CheckArgumentIsNull(nameof(_contextAccessor));

            _roleService = roleService;
            _roleService.CheckArgumentIsNull(nameof(_roleService));
        }

        public Task<User> FindUserAsync(int userId)
        {
            return _users.FindAsync(userId);
        }

        public Task<User> FindUserAsync(string username, string password)
        {
            var passwordHash = _securityService.GetSha256Hash(password);
            return _users.FirstOrDefaultAsync(x => x.Username == username && x.Password == passwordHash);
        }

        public Task<User> FindUserAsync(string username)
        {
            return _users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<string> GetSerialNumberAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            return user.SerialNumber;
        }

        public async Task UpdateUserLastActivityDateAsync(int userId)
        {
            var user = await FindUserAsync(userId);
            if (user.LastVisitDate != null)
            {
                var updateLastActivityDate = TimeSpan.FromMinutes(2);
                var currentUtc = DateTimeOffset.UtcNow;
                var timeElapsed = currentUtc.Subtract(user.LastVisitDate.Value);
                if (timeElapsed < updateLastActivityDate)
                {
                    return;
                }
            }
            user.LastVisitDate = DateTimeOffset.UtcNow;
            await _uow.SaveChangesAsync();
        }

        public string GetCurrentUserDisplayName()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst("DisplayName");
            var userDisplayName = userDataClaim?.Value;
            return userDisplayName;
        }
        public string GetCurrentUserUsername()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            var userUserName = userDataClaim?.Value;
            return userUserName;
        }

        public int GetCurrentUserId()
        {
            var claimsIdentity = _contextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var userDataClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
            var userId = userDataClaim?.Value;
            return string.IsNullOrWhiteSpace(userId) ? 0 : int.Parse(userId);
        }

        public Task<User> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            return FindUserAsync(userId);
        }

        public async Task<(bool Succeeded, string Error)> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            var currentPasswordHash = _securityService.GetSha256Hash(currentPassword);
            if (user.Password != currentPasswordHash)
            {
                return (false, "Current password is wrong.");
            }

            user.Password = _securityService.GetSha256Hash(newPassword);
            // user.SerialNumber = Guid.NewGuid().ToString("N"); // To force other logins to expire.
            await _uow.SaveChangesAsync();
            return (true, string.Empty);
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            return _users.ToListAsync();
        }

        public async Task<(bool Succeeded, string Error)> CreateUserAsync(User user, string password)
        {
            //Todo: need optimization
            if (await _users.AnyAsync(q => q.Username == user.Username))
            {
                return (false, "This username is already taken.");
            }
            if (await _users.AnyAsync(q => q.Email == user.Email))
            {
                return (false, "This Email is already taken.");
            }
            if (await _users.AnyAsync(q => q.PhoneNumber == user.PhoneNumber))
            {
                return (false, "This Phone is already taken.");
            }
            user.Password = _securityService.GetSha256Hash(password);
            user.SerialNumber = Guid.NewGuid().ToString("N");
            _users.Add(user);
            await _uow.SaveChangesAsync();

            await _roleService.AddUserInRoleAsync(user, CustomRoles.User);

            return (true, string.Empty);
        }

    }
}
