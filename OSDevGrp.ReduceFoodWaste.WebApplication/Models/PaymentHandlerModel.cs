using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a data provider who can handle payments.
    /// </summary>
    [Serializable]
    public class PaymentHandlerModel : DataProviderModel
    {
        /// <summary>
        /// Gets or sets the action name which starts the payment process handled by the data provider.
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// Gets or sets the path to the images which descibes the data provider who can handle payments.
        /// </summary>
        public string ImagePath { get; set; }
    }
}