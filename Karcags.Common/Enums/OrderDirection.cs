using System;

namespace Karcags.Common.Enums
{
    /// <summary>
    /// Order Direction
    /// </summary>
    public enum OrderDirection
    {
        /// <value>
        /// Ascend ordering
        /// </value>
        Ascend = 1,
        
        /// <value>
        /// Descend ordering
        /// </value>
        Descend = 2,
        
        /// <value>
        /// Not ordered
        /// </value>
        None = 3
    }

    /// <summary>
    /// Order Direction Service.
    /// Manage orderings
    /// </summary>
    public static class OrderDirectionService
    {
        /// <summary>
        /// Get string value of ordering
        /// </summary>
        /// <param name="direction">Ordering direction</param>
        /// <returns>Ordering direction as string</returns>
        /// <exception cref="ArgumentException">When the Ordering is invalid</exception>
        public static string GetValue(OrderDirection direction)
        {
            return direction switch
            {
                OrderDirection.Ascend => "asc",
                OrderDirection.Descend => "desc",
                OrderDirection.None => "none",
                _ => throw new ArgumentException("Direction does not exist")
            };
        }

        /// <summary>
        /// Convert string value to Ordering
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Order direction</returns>
        /// <exception cref="ArgumentException">When string is invalid</exception>
        public static OrderDirection ValueToKey(string value)
        {
            return value switch
            {
                "asc" => OrderDirection.Ascend,
                "desc" => OrderDirection.Descend,
                "none" => OrderDirection.None,
                _ => throw new ArgumentException("Value does not exist")
            };
        }
    }
}