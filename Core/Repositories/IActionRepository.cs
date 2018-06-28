using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IActionRepository
    {
        Task<IdentityResult> AddAction(Model.Identity.Action action);
        Task<IdentityResult> UpdateRole(Model.Identity.Action action);
        Task<IdentityResult> DeleteRole(Model.Identity.Action action);
        Task<IEnumerable<Model.Identity.Action>> GetAllRoles();
        Task<Model.Identity.Action> GetAction(int actionId);
        Task<Model.Identity.Action> GetAction(string actionName);
    }
}
