﻿using System;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Payments;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model for a data provider who provides data.
    /// </summary>
    public class DataProviderModel : IPaymentHandler
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier for the data provider.
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Gets or sets the name of data provider.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets statement for the data source who are provided by the data provider.
        /// </summary>
        public string DataSourceStatement { get; set; }

        #endregion 
    }
}