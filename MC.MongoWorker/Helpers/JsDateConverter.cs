using System;
using System.Collections.Generic;
using System.Text;

namespace MC.MongoWorker.Helpers
{
    public static class JsDateConverter
    {

        private static DateTime _jan1st1970 = new DateTime(1970, 1, 1);

        public static long Convert(DateTime from)
        {
            return System.Convert.ToInt64((from - _jan1st1970).TotalMilliseconds);
        }

        public static DateTime Convert(long from)
        {
            return _jan1st1970.AddMilliseconds(from);
        }

    }
}
