using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoCoreNet.Authorization
{
    public interface IAuthtenticationCurrentContext {
        void setCurrentUser(string user);
        void setCurrentRoleId(string roleId);
        string CurrentUser { get; }

        void setDefault();

    }
    public class AuthenticationCurrentContext : IAuthtenticationCurrentContext
    {
        private string UserId { get; set; }
        private int RoleTypeId { get; set; }

        public void setCurrentUser(string user) {
            UserId = user;
        }

        public void setCurrentRoleId(string roleId) {
            RoleTypeId = Convert.ToInt32(roleId);
        }

        public string CurrentUser {
            get {
                return UserId;
            }
        }

        public void setDefault() {
            UserId = null;
        }
    }
}
