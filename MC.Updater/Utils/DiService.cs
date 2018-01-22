using System;
using System.Collections.Generic;
using System.Text;

namespace MC.Updater.Utils
{
    public static class DiService
    {

        public static T GetServiceInstance<T>(this IServiceProvider service)
        {
            return (T)service.GetService(typeof(T));

        }

    }
}
