﻿using System;

namespace Model
{
    /// <summary>
    /// Uniquely Typed List, list of objects that can be of any type.
    /// </summary>
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
                return default;
            }
        }
    }
}
