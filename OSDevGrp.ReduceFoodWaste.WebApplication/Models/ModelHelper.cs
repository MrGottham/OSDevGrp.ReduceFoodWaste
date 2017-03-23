using System;
using System.IO;
using System.IO.Compression;
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

            using (MemoryStream serializeStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(serializeStream, CompressionMode.Compress))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(gZipStream, model);
                }
                return Convert.ToBase64String(serializeStream.ToArray());
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

            using (MemoryStream decompressStream = new MemoryStream(Convert.FromBase64String(encodedModel)))
            {
                using (GZipStream gZipStream = new GZipStream(decompressStream, CompressionMode.Decompress))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    return binaryFormatter.Deserialize(gZipStream);
                }
            }
        }
    }
}
