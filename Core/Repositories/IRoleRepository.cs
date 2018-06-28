using Core.Model.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IRoleRepository
    {
        Task<IdentityResult> AddRole(ApplicationRole role);
        Task<IdentityResult> UpdateRole(ApplicationRole role);
        Task<IdentityResult> DeleteRole(ApplicationRole role);
        Task<IEnumerable<ApplicationRole>> GetAllRoles();
        Task<ApplicationRole> GetRole(int roleId);
        Task<ApplicationRole> GetRole(string roleName);
    }
}
