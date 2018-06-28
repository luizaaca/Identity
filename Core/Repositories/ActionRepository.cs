using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Identity;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Core.Repositories
{
    public class ActionRepository : BaseRepository, IActionRepository
    {
        public async Task<IdentityResult> AddAction(Model.Identity.Action action)
        {
            if (await connection.InsertAsync(action) > 0) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert action {action.Name}." });
        }

        public async Task<IdentityResult> DeleteRole(Model.Identity.Action action)
        {
            action.Active = false;

            if (await connection.UpdateAsync(action)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete action {action.Name}." });
        }

        public async Task<Model.Identity.Action> GetAction(int actionId)
        {
            return await connection.GetAsync<Model.Identity.Action>(actionId);
        }

        public async Task<Model.Identity.Action> GetAction(string actionName)
        {
            var sql = @"select * from Actions where Name = @Name AND Active = 1";
            return await connection.QuerySingleOrDefaultAsync<Model.Identity.Action>(sql, new { Name = actionName });
        }

        public async Task<IEnumerable<Model.Identity.Action>> GetAllRoles()
        {
            return await connection.GetAllAsync<Model.Identity.Action>();
        }

        public async Task<IdentityResult> UpdateRole(Model.Identity.Action action)
        {
            if (await connection.UpdateAsync(action)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not update action {action.Name}." });
        }
    }
}
