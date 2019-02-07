using Microsoft.EntityFrameworkCore;
using PizzaOffer.Common;
using PizzaOffer.DataLayer.Context;
using PizzaOffer.DomainClasses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOffer.Services
{
    public interface IRolesService
    {
        Task<List<Role>> FindUserRolesAsync(int userId);
        Task<bool> IsUserInRoleAsync(int userId, string roleName);
        Task<List<User>> FindUsersInRoleAsync(string roleName);
        Task AddUserInRoleAsync(int userId, string roleName);
        Task AddUserInRoleAsync(User user, string roleName);
        Task AddUserInRoleAsync(User user, Role role);
    }

    public class RolesService : IRolesService
    {
        private readonly IUnitOfWork _uow;
        private readonly DbSet<Role> _roles;
        private readonly DbSet<User> _users;
        private readonly DbSet<UserRole> _userRole;

        public RolesService(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.CheckArgumentIsNull(nameof(_uow));

            _roles = _uow.Set<Role>();
            _users = _uow.Set<User>();
            _userRole = _uow.Set<UserRole>();
        }

        public Task<List<Role>> FindUserRolesAsync(int userId)
        {
            var userRolesQuery = from role in _roles
                                 from userRoles in role.UserRoles
                                 where userRoles.UserId == userId
                                 select role;

            return userRolesQuery.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> IsUserInRoleAsync(int userId, string roleName)
        {
            var userRolesQuery = from role in _roles
                                 where role.Name == roleName
                                 from user in role.UserRoles
                                 where user.UserId == userId
                                 select role;
            var userRole = await userRolesQuery.FirstOrDefaultAsync();
            return userRole != null;
        }

        public Task<List<User>> FindUsersInRoleAsync(string roleName)
        {
            var roleUserIdsQuery = from role in _roles
                                   where role.Name == roleName
                                   from user in role.UserRoles
                                   select user.UserId;
            return _users.Where(user => roleUserIdsQuery.Contains(user.Id))
                         .ToListAsync();
        }

        public async Task AddUserInRoleAsync(int userId, string roleName)
        {
            var role = await _roles.FirstOrDefaultAsync(q => q.Name == roleName);
            var user = await _users.FirstOrDefaultAsync(q => q.Id == userId);
            await AddUserInRoleAsync(user, role);
        }

        public async Task AddUserInRoleAsync(User user, string roleName)
        {
            var role = await _roles.FirstOrDefaultAsync(q => q.Name == roleName);
            await AddUserInRoleAsync(user, role);
        }

        public async Task AddUserInRoleAsync(User user, Role role)
        {
            _userRole.Add(new UserRole { User = user, Role = role });
            await _uow.SaveChangesAsync();
        }
    }
}
