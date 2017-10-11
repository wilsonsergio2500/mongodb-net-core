using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MC.JwToken.ModelToken
{
    public interface IModelTokenGenerator<T>
    {
        void Create(T entity, Dictionary<string, Func<T, object>> items);
        ClaimsIdentity getClaims { get; }

    }
}
