using MC.JwToken;
using MC.JwToken.ModelToken;
using MC.JwToken.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MongoCoreNet.Authorization
{

    public class AuthorizationTokenRequirement : IAuthorizationRequirement {

    }

    public class AuthorizationTokenPresent : AuthorizationHandler<AuthorizationTokenRequirement>
    {
        private readonly IJwtSecurityProvider tokenProvider;
        private readonly IAuthtenticationCurrentContext currentAuthenticationContext;

        public AuthorizationTokenPresent(IJwtSecurityProvider tokenp, IAuthtenticationCurrentContext currentAuthContext)
        {
            tokenProvider = tokenp;
            currentAuthenticationContext = currentAuthContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationTokenRequirement requirement)
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
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();
                }
            }
           

            return Task.CompletedTask;
        }
    }


}
