using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.CustomAttributes
{
    public class RequiresPermissionAttribute : TypeFilterAttribute
    {
        public RequiresPermissionAttribute(params string[] permissions)
          : base(typeof(RequiresPermissionAttributeImpl))
        {
            Arguments = new[] { new PermissionsAuthorizationRequirement(permissions) };
        }

        private class RequiresPermissionAttributeImpl : Attribute, IAsyncResourceFilter
        {
            private readonly ILogger _logger;
            private readonly IAuthorizationService _authService;
            private readonly PermissionsAuthorizationRequirement _requiredPermissions;

            public RequiresPermissionAttributeImpl(ILogger<RequiresPermissionAttribute> logger,
                                            IAuthorizationService authService,
                                            PermissionsAuthorizationRequirement requiredPermissions)
            {
                _logger = logger;
                _authService = authService;
                _requiredPermissions = requiredPermissions;
            }

            public async Task OnResourceExecutionAsync(ResourceExecutingContext context,
                                                       ResourceExecutionDelegate next)
            {
                if (!(await _authService.AuthorizeAsync(context.HttpContext.User,
                                            null,
                                            _requiredPermissions)).Succeeded)
                {
                    context.Result = new ChallengeResult();
                }

                await next();
            }
        }
    }

    public class PermissionsAuthorizationRequirement : DoesUserHavePermissionAuthorizationHandler, IAuthorizationRequirement
    {
        public IEnumerable<string> RequiredPermissions { get; }

        public PermissionsAuthorizationRequirement(IEnumerable<string> requiredPermissions)
        {
            RequiredPermissions = requiredPermissions;
        }
    }

    public class DoesUserHavePermissionAuthorizationHandler : AuthorizationHandler<PermissionsAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionsAuthorizationRequirement requirement)
        {
            if (requirement.RequiredPermissions.All(p => context.User.HasClaim(claim => claim.Type == "permission" && claim.Value == p)))
                context.Succeed(requirement);
            else
                context.Fail();

            return Task.FromResult(0);
        }
    }
}
