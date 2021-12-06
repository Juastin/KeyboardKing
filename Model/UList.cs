using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class UList
    {
        private object[] _data {get;set;}

        public UList(object[] data)
        {
            _data = data;
        }

        public T Get<T>(int index)
        {
            return (T)Convert.ChangeType(_data[index], typeof(T));
        }
    }
}
