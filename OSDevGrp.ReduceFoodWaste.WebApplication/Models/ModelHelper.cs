using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model helper.
    /// </summary>
    public class ModelHelper : IModelHelper
    {
        /// <summary>
        /// Encodes and returns a given type's name.
        /// </summary>
        /// <param name="type">Type for which to encode and return the name.</param>
        /// <returns>Encoded value for the given type's name.</returns>
        public string ToBase64(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return Convert.ToBase64String(Encoding.Default.GetBytes(type.FullName));
        }

        /// <summary>
        /// Decodes and returns the type for a given encoded type name.
        /// </summary>
        /// <param name="encodedTypeName">Encoded type name.</param>
        /// <returns>Type for the given encoded type name.</returns>
        public Type ToType(string encodedTypeName)
        {
            if (string.IsNullOrEmpty(encodedTypeName))
            {
                throw new ArgumentNullException(nameof(encodedTypeName));
            }

            return GetType().Assembly.GetType(Encoding.Default.GetString(Convert.FromBase64String(encodedTypeName)), true);
        }

        /// <summary>
        /// Serializes a given model and returns the encoded value for it.
        /// </summary>
        /// <param name="model">Model which should be serialized and returned as an encoded value.</param>
        /// <returns>Encoded value for the serialized model.</returns>
        public string ToBase64(object model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, model);

                memoryStream.Seek(0, SeekOrigin.Begin);
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        /// <summary>
        /// Deserialize and returns a the model from a given encoded model.
        /// </summary>
        /// <param name="encodedModel">Encoded model.</param>
        /// <returns>Model for the given encoded model.</returns>
        public object ToModel(string encodedModel)
        {
            if (string.IsNullOrEmpty(encodedModel))
            {
                throw new ArgumentNullException(nameof(encodedModel));
            }

            using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encodedModel)))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}