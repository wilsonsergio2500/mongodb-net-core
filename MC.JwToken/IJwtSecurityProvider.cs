using MC.JwToken.ModelToken;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;

namespace MC.JwToken
{
    public interface IJwtSecurityProvider
    {
        string WriteToken(ClaimsIdentity Identity);

        string WriteToken<T>(IModelTokenGenerator<T> tokenGenerator);

        ReadOnlyCollection<Claim> GetClaimsCollection(string encodedToken);
        bool IsTokenValid(string encodedToken);


     }
        
}
