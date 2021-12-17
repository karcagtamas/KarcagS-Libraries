using System;

namespace Karcags.Common.Tools.Export
{
    /// <summary>
    /// Table header object
    /// </summary>
    public class Header
    {
        private string PropertyName { get; set; }
        public string DisplayName { get; set; }
        private Func<object, string> Displaying { get; set; }

        /// <summary>
        /// Header init
        /// </summary>
        /// <param name="property">Property name</param>
        /// <param name="display">Display name</param>
        /// <param name="displaying">Displaying method</param>
        public Header(string property, string display, Func<object, string> displaying)
        {
            this.PropertyName = property;
            this.DisplayName = display;
            this.Displaying = displaying;
        }

        /// <summary>
        /// Get string value from the given object
        /// </summary>
        /// <param name="obj">Source object</param>
        /// <typeparam name="T">Type of the object</typeparam>
        /// <returns>Current property's value</returns>
        public string GetValue<T>(T obj)
        {
            var type = obj.GetType();
            var property = type.GetProperty(this.PropertyName);

            return property != null ? this.Displaying(property.GetValue(obj)) : "";
        }
    }
}