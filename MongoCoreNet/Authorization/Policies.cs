using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Authorization
{
    public static class Policies
    {
        public const string AUTHORIZATION_TOKEN = "AuthorizationToken";
        public const string AUTHORIZATION_ADMIN_ONLY = "AuthorizationAdminOnly";
    }
}
