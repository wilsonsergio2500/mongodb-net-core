using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MC.JwToken.ModelToken
{
    public static class ClaimKeys
    {
        public const string USER_ID = "userId";
        public const string ROLE = ClaimTypes.Role;
    }
}
