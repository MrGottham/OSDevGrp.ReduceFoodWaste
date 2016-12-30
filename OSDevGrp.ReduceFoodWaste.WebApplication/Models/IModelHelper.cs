using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Inteface to a model helper.
    /// </summary>
    public interface IModelHelper
    {
        /// <summary>
        /// Encodes and returns a given type's name.
        /// </summary>
        /// <param name="type">Type for which to encode and return the name.</param>
        /// <returns>Encoded value for the given type's name.</returns>
        string ToBase64(Type type);

        /// <summary>
        /// Decodes and returns the type for a given encoded type name.
        /// </summary>
        /// <param name="encodedTypeName">Encoded type name.</param>
        /// <returns>Type for the given encoded type name.</returns>
        Type ToType(string encodedTypeName);

        /// <summary>
        /// Serializes a given model and returns the encoded value for it.
        /// </summary>
        /// <param name="model">Model which should be serialized and returned as an encoded value.</param>
        /// <returns>Encoded value for the serialized model.</returns>
        string ToBase64(object model);

        /// <summary>
        /// Deserialize and returns a given model from a given encoded model.
        /// </summary>
        /// <param name="encodedModel">Encoded model.</param>
        /// <returns>Model for the given encoded model.</returns>
        object ToModel(string encodedModel);
    }
}
