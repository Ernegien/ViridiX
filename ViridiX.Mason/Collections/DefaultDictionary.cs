using System;
using System.Collections.Generic;

namespace ViridiX.Mason.Collections
{
    /// <summary>
    /// Represents a standard dictionary of key-value pairs with a default key used for implicit casting to a value.
    /// </summary>
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// The default key.
        /// </summary>
        public TKey DefaultKey { get; }

        /// <summary>
        /// Initializes a dictionary with the specified default key.
        /// </summary>
        /// <param name="defaultKey"></param>
        public DefaultDictionary(TKey defaultKey)
        {
            if (defaultKey == null)
                throw new ArgumentNullException(nameof(defaultKey));

            DefaultKey = defaultKey;
        }

        /// <summary>
        /// Returns the value of the default key.
        /// </summary>
        /// <param name="dictionary"></param>
        public static implicit operator TValue(DefaultDictionary<TKey, TValue> dictionary)
        {
            return dictionary[dictionary.DefaultKey];
        }
    }
}
