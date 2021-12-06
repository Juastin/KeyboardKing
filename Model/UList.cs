using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{

    public class UList
    {
        /// <summary>
        /// Property for the content of the list.
        /// </summary>
        private object[] _data {get;set;}

        public UList(object[] data)
        {
            _data = data;
        }

        /// <summary>
        /// Method to get and cast an object from the _data array.
        /// T is the expected type.
        /// </summary>
        public T Get<T>(int index)
        {
            try
            {
                return (T)Convert.ChangeType(_data[index], typeof(T));
            } catch
            {
                throw new Exception("Given generic type does not match the object origin type.");
            }
        }
    }
}
