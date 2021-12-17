using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Karcags.Common.Tools
{
    /// <summary>
    /// Serializable object copier
    /// </summary>
    public static class ObjectCopier
    {
        /// <summary>
        /// Clone
        /// </summary>
        /// <param name="source">Source object</param>
        /// <typeparam name="T">Type of object</typeparam>
        /// <returns></returns>
        public static T Clone<T>(T source)
        {
            // Check serializable state
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Object is null
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            // Cloning
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}