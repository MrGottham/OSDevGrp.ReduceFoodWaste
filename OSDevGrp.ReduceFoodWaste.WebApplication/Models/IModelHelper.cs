namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Inteface to a model helper.
    /// </summary>
    public interface IModelHelper
    {
        /// <summary>
        /// Serializes a given model and returns the base64 encoded value for it.
        /// </summary>
        /// <param name="model">Model which should be serialized and for which the base64 encoded value should be returned.</param>
        /// <returns>Base64 encoded value for the serialized model.</returns>
        string ToBase64(object model);

        /// <summary>
        /// Deserialize and returns a given model from a given base64 encoded model.
        /// </summary>
        /// <param name="encodedModel">Base64 encoded model.</param>
        /// <returns>Model for the given base64 encoded model.</returns>
        object ToModel(string encodedModel);
    }
}
