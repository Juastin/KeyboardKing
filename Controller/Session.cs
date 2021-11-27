using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class Session
    {
        private static Dictionary<string, object> Data {get;set;} = new Dictionary<string, object>();

        public static object Get(string key)
        {
            return Data.ContainsKey(key) ? Data[key] : null;
        }

        public static bool Add(string key, object o)
        {
            if (Data.ContainsKey(key))
                Data.Remove(key);

            Data.Add(key, o);
                return true;
        }

        public static bool Remove(string key)
        {
            if (Data.ContainsKey(key))
            {
                Data.Remove(key);
                return true;
            } else
            {
                return true;
            }
        }

        public static void Flush()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
