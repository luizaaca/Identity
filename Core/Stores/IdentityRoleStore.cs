using Core.Model.Identity;
using Core.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Stores
{
    public class IdentityRoleStore : IRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>
    {
        private IRoleRepository _roleRepository;
        private IActionRepository _actionRepository;

        public IdentityRoleStore(IRoleRepository roleRepository, IActionRepository actionRepository)
        {
            _roleRepository = roleRepository;
            _actionRepository = actionRepository;
        }
        public Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            var action = _actionRepository.GetAction(claim.Value).Result;

            if (action == null) throw new ArgumentNullException(nameof(action));

            if (!role.RoleActions.Exists(r => r.Name == action.Name)) role.RoleActions.Add(action);

            _roleRepository.UpdateRole(role);

            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return await _roleRepository.AddRole(role);
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return await _roleRepository.DeleteRole(role);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (roleId == null) throw new ArgumentNullException(nameof(roleId));

            if (!int.TryParse(roleId, out int id))
            {
                throw new ArgumentException("Not a valid int id", nameof(roleId));
            }

            return await _roleRepository.GetRole(id);
        }

        public async Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (normalizedRoleName == null) throw new ArgumentNullException(nameof(normalizedRoleName));

            return await _roleRepository.GetRole(normalizedRoleName);
        }

        public Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult((IList<Claim>)role.RoleActions.Select(a => new Claim("permission", a.Name)).ToList());
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));

            return Task.FromResult(role.Name);
        }

        public Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (role == null) throw new ArgumentNullException(nameof(role));
            if (claim == null) throw new ArgumentNullException(nameof(claim));

            role.RoleActions.Remove(role.RoleActions.Where(a => a.Name == claim.Value).SingleOrDefault());

            return Task.FromResult<object>(null);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            //cancellationToken.ThrowIfCancellationRequested();

            //if (role == null) throw new ArgumentNullException(nameof(role));
            //role.Name = normalizedName ?? throw new ArgumentNullException(nameof(normalizedName));

            //return Task.FromResult<object>(null);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
