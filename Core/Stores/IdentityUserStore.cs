using Core.Model.Identity;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Stores
{
    public class IdentityUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserRoleStore<ApplicationUser>
    {
        IUserRepository _userRepository;
        IRoleRepository _roleRepository;

        public IdentityUserStore(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task AddToRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            var role = await _roleRepository.GetRole(roleName);

            if (role == null) throw new ArgumentNullException(nameof(role));

            //if (!user.Roles.Exists(r => r.Name == roleName)) user.Roles.Add(role);

            _userRepository.AddUserRole(user.Id, role.Id);

        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            user.Active = true;

            return await _userRepository.AddUser(user);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return await _userRepository.DeleteUser(user);
        }

        public void Dispose()
        {
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (normalizedEmail == null) throw new ArgumentNullException(nameof(normalizedEmail));

            return await _userRepository.FindUserByEmail(normalizedEmail);
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userId == null) throw new ArgumentNullException(nameof(userId));

            if (!int.TryParse(userId, out int id))
            {
                throw new ArgumentException("Not a valid int id", nameof(userId));
            }

            return await _userRepository.GetUser(id);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (userName == null) throw new ArgumentNullException(nameof(userName));

            return await _userRepository.GetUser(userName);
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return (await _userRepository.GetUserRoles(user.Id)).Select(r => r.Name).ToList();
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return (await _userRepository.GetUserRoles(user.Id)).ToList().Exists(r => r.Name == roleName);
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));
            user.PasswordHash = email ?? throw new ArgumentNullException(nameof(email));

            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedEmail == null) throw new ArgumentNullException(nameof(normalizedEmail));

            user.Email = normalizedEmail;

            return Task.FromResult<object>(null);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));

            user.Name = normalizedName;

            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (user == null) throw new ArgumentNullException(nameof(user));

            user.PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));

            return Task.FromResult<object>(null);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            user.UserName = userName ?? throw new ArgumentNullException(nameof(userName));
            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            return await _userRepository.UpdateUser(user);
        }
    }
}
