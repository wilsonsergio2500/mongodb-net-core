using System;
using System.Threading.Tasks;
using MC.JwToken;
using Microsoft.AspNetCore.Authorization;
using Mdls = MC.Models;
using System.Collections.ObjectModel;
using System.Security.Claims;
using MC.JwToken.Helpers;
using MC.JwToken.ModelToken;

namespace MongoCoreNet.Authorization
{
    public class AdminRoleOnlyAuthorizationRequirment : IAuthorizationRequirement
    {

    }

    public class AdminRoleAuthorization : AuthorizationHandler<AdminRoleOnlyAuthorizationRequirment>
    {

        private readonly IJwtSecurityProvider tokenProvider;
        private readonly IAuthtenticationCurrentContext currentAuthenticationContext;

        public AdminRoleAuthorization(IJwtSecurityProvider tokenp, IAuthtenticationCurrentContext currentAuthContext) 
        {
            tokenProvider = tokenp;
            currentAuthenticationContext = currentAuthContext;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleOnlyAuthorizationRequirment requirement)
        {
            Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext resource = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;
            Microsoft.AspNetCore.Http.DefaultHttpContext httpContext = resource.HttpContext as Microsoft.AspNetCore.Http.DefaultHttpContext;
            Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest request = httpContext.Request as Microsoft.AspNetCore.Http.Internal.DefaultHttpRequest;
            Microsoft.AspNetCore.Server.Kestrel.Internal.Http.FrameRequestHeaders headers = request.Headers as Microsoft.AspNetCore.Server.Kestrel.Internal.Http.FrameRequestHeaders;

            string AuthorizationToken = headers.HeaderAuthorization;

            if (String.IsNullOrEmpty(AuthorizationToken))
            {
                context.Fail();
            }
            else
            {
                string authvalue = AuthorizationToken.Replace("Bearer ", "");
                bool isTokenValid = tokenProvider.IsTokenValid(authvalue);
                if (isTokenValid)
                {
                    ReadOnlyCollection<Claim> claims = tokenProvider.GetClaimsCollection(authvalue);
                    currentAuthenticationContext.setCurrentUser(claims.GetKey(ClaimKeys.USER_ID));
                    currentAuthenticationContext.setCurrentRoleId(claims.GetKey(ClaimKeys.ROLE));
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }


            if ((Mdls.RoleType)currentAuthenticationContext.CurrentRoleId != Mdls.RoleType.Administrator)
            {
                context.Fail();
            }


            return Task.CompletedTask;
        }
    }
}
