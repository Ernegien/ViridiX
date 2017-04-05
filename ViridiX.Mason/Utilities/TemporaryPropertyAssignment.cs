using System;
using System.Diagnostics;
using System.Reflection;

namespace ViridiX.Mason.Utilities
{
    /// <summary>
    /// Allows the assignment of a temporary value to the specified object and property which will be restored to the original value upon disposal.
    /// Do not use for critical path code due to the slowness of the reflection within.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemporaryPropertyAssignment<T> : IDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly object _propertyObject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly T _originalValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PropertyInfo _propertyInfo;

        /// <summary>
        /// Assigns a temporary value to the specified object property.
        /// </summary>
        /// <param name="propertyObject"></param>
        /// <param name="propertyName"></param>
        /// <param name="temporaryValue"></param>
        public TemporaryPropertyAssignment(object propertyObject, string propertyName, T temporaryValue)
        {
            // cache the property info
            _propertyInfo = propertyObject.GetType().GetProperty(propertyName);
           
            // store reference to original
            _propertyObject = propertyObject;
            
            // store original value
            _originalValue = (T)_propertyInfo.GetValue(_propertyObject, null);

            // modify original with temp value
            _propertyInfo.SetValue(_propertyObject, temporaryValue);
        }

        /// <summary>
        /// Restores the original value.
        /// </summary>
        public void Dispose()
        {
            // restore the original value
            _propertyInfo.SetValue(_propertyObject, _originalValue);
        }
    }
}
