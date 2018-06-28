using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Model.Identity;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Core.Repositories
{
    public class RoleRepository : BaseRepository, IRoleRepository
    {
        public async Task<IdentityResult> AddRole(ApplicationRole role)
        {
            if (await connection.InsertAsync(role) > 0) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not insert role {role.Name}." });
        }

        public async Task<IdentityResult> DeleteRole(ApplicationRole role)
        {
            role.Active = false;

            if (await connection.UpdateAsync(role)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.Name}." });
        }

        public async Task<IEnumerable<ApplicationRole>> GetAllRoles()
        {
            return await Task.Run(() =>
            {
                var roleDictionary = new Dictionary<int, ApplicationRole>();

                var sql = @"select A.*, C.* from Roles A
                        inner join RolesActions B on A.Id = B.RoleId AND B.Active = 1
                        inner join Actions C on C.Id = ActionId AND C.Active = 1
                        where A.Active = 1";

                return connection.Query<ApplicationRole, Model.Identity.Action, ApplicationRole>(sql,
                    (role, action) =>
                    {
                        if (!roleDictionary.TryGetValue(role.Id, out ApplicationRole roleEntry))
                        {
                            roleEntry = role;
                            roleEntry.RoleActions = new List<Model.Identity.Action>();
                            roleDictionary.Add(roleEntry.Id, roleEntry);
                        }

                        roleEntry.RoleActions.Add(action);

                        return roleEntry;
                    },
                    splitOn: "ActionId")
                .Distinct()
                .ToList();
            });
        }

        public async Task<ApplicationRole> GetRole(int roleId)
        {
            return await Task.Run(() =>
            {
                var roleDictionary = new Dictionary<int, ApplicationRole>();

                var sql = @"select A.*, C.* from Roles A
                        inner join RolesActions B on A.Id = B.RoleId AND B.Active = 1
                        inner join Actions C on C.Id = ActionId AND C.Active = 1
                        where A.[Id] = @Id and A.Active = 1";

                return connection.Query<ApplicationRole, Model.Identity.Action, ApplicationRole>(sql,
                    (role, action) =>
                    {
                        if (!roleDictionary.TryGetValue(role.Id, out ApplicationRole roleEntry))
                        {
                            roleEntry = role;
                            roleEntry.RoleActions = new List<Model.Identity.Action>();
                            roleDictionary.Add(roleEntry.Id, roleEntry);
                        }

                        roleEntry.RoleActions.Add(action);

                        return roleEntry;
                    },
                    new { Id = roleId },
                    splitOn: "ActionId")
                .Distinct()
                .SingleOrDefault();
            });
        }

        public async Task<ApplicationRole> GetRole(string roleName)
        {
            return await Task.Run(() =>
            {
                var roleDictionary = new Dictionary<int, ApplicationRole>();

                var sql = @"select A.*, C.Id as ActionId, C.* from Roles A
                        inner join RolesActions B on A.Id = B.RoleId AND B.Active = 1
                        inner join Actions C on C.Id = ActionId AND C.Active = 1
                        where A.[Name] = @Name and A.Active = 1";

                return connection.Query<ApplicationRole, Model.Identity.Action, ApplicationRole>(sql,
                    (role, action) =>
                    {
                        if (!roleDictionary.TryGetValue(role.Id, out ApplicationRole roleEntry))
                        {
                            roleEntry = role;
                            roleEntry.RoleActions = new List<Model.Identity.Action>();
                            roleDictionary.Add(roleEntry.Id, roleEntry);
                        }

                        roleEntry.RoleActions.Add(action);

                        return roleEntry;
                    },
                    new { Name = roleName },
                    splitOn: "ActionId")
                .Distinct()
                .SingleOrDefault();
            });
        }

        public async Task<IdentityResult> UpdateRole(ApplicationRole role)
        {
            if (await connection.UpdateAsync(role)) return IdentityResult.Success;

            return IdentityResult.Failed(new IdentityError { Description = $"Could not update role {role.Name}." });
        }
    }
}
