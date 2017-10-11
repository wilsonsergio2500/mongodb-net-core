using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Linq;
using System.Text;

namespace MC.JwToken.Helpers
{
    public static class ClaimFinder
    {
        public static string GetKey(this ReadOnlyCollection<Claim> claims, string key)
        {
            return claims.First(g => g.Type == key).Value;
        }
    }
}
