using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model helper.
    /// </summary>
    public class ModelHelper : IModelHelper
    {
        /// <summary>
        /// Serializes a given model and returns the base64 encoded value for it.
        /// </summary>
        /// <param name="model">Model which should be serialized and for which the base64 encoded value should be returned.</param>
        /// <returns>Base64 encoded value for the serialized model.</returns>
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
        /// Deserialize and returns a given model from a given base64 encoded model.
        /// </summary>
        /// <param name="encodedModel">Base64 encoded model.</param>
        /// <returns>Model for the given base64 encoded model.</returns>
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
