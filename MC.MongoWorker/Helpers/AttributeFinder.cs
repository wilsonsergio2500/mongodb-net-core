using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;

namespace MC.MongoWorker.Helpers
{
    public static class AttributeFinder
    {

        public delegate TResult GetValue_t<in T, out TResult>(T arg1);
        public static TValue GetAttributValue<TAttribute, TValue>(System.Reflection.MemberInfo mi, GetValue_t<TAttribute, TValue> value) where TAttribute : System.Attribute
        {
            
            TAttribute[] objAtts = (TAttribute[])mi.GetCustomAttributes(typeof(TAttribute), true);
            TAttribute att = (objAtts == null || objAtts.Length < 1) ? default(TAttribute) : objAtts[0];

            if (att != null)
            {
                return value(att);
            }
            return default(TValue);
        }


        public static TValue GetAttributeValue<T1, TAttribute, TValue>( Func<TAttribute, TValue> valueSelector)
        where TAttribute : Attribute
        {
            
            var att = typeof(T1).GetTypeInfo().GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

    }
}
