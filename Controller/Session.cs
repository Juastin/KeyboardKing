using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public static class Session
    {

        public static event EventHandler PropertyChanged;

        private static Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();

        public static object Get(string key)
        {
            return Data.ContainsKey(key) ? Data[key] : null;
        }

        public static bool Add(string key, object o)
        {
            if (Data.ContainsKey(key))
                Data.Remove(key);

            Data.Add(key, o);
            PropertyChanged?.Invoke(null, EventArgs.Empty);
            return true;
        }

        public static bool Remove(string key)
        {
            if (!Data.ContainsKey(key)) return false;

            Data.Remove(key);
            return true;

        }

        public static void Flush()
        {
            Data = new Dictionary<string, object>();
        }
    }
}
