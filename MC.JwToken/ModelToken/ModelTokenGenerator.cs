
using MC.Models.Base;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace MC.JwToken.ModelToken
{
    public class ModelTokenGenerator<T> : IModelTokenGenerator<T> where T : BaseEntity, new()
    {

        private Dictionary<string, Func<T, object>> Items = new Dictionary<string, Func<T, object>>();
        private T Entity;

        public ModelTokenGenerator() {
           
        }

        public void Create(T entity, Dictionary<string, Func<T, object>> items) {
            Entity = entity;
            Items = items;
        }

        public ClaimsIdentity getClaims {

            get {
                if (Items.Count == 0) {
                    throw new Exception("model Creator must be created with claim items delagetes");
                }

                ClaimsIdentity identity = new ClaimsIdentity();
                foreach (KeyValuePair<string, Func<T, object>> item in Items) {

                    string value = item.Value.Invoke(Entity).ToString();
                    identity.AddClaim(new Claim(item.Key, value));
                }

                return identity;
            }
        }

    }
}
