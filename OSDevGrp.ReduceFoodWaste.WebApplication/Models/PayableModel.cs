﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models
{
    /// <summary>
    /// Model on which it's possible to make a payment.
    /// </summary>
    [Serializable]
    public class PayableModel
    {
        #region Private variables

        private string _billingInformation;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the billing information for the payable model.
        /// </summary>
        public virtual string BillingInformation
        {
            get
            {
                if (_billingInformation == null)
                {
                    return _billingInformation;
                }
                return _billingInformation
                    .Replace("[Price]", Price.ToString("C", PriceCultureInfo));
            }
            set { _billingInformation = value; }
        }

        /// <summary>
        /// Gets or sets the price of the payable model.
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the name of the culture informations for the price of the payable model.
        /// </summary>
        public virtual string PriceCultureInfoName { get; set; }

        /// <summary>
        /// Gets the culture informations for the price of the payable model.
        /// </summary>
        public virtual CultureInfo PriceCultureInfo
        {
            get
            {
                return string.IsNullOrWhiteSpace(PriceCultureInfoName) ? CultureInfo.CurrentUICulture : new CultureInfo(PriceCultureInfoName);
            }
        }

        /// <summary>
        /// Gets whether the payable model is free of cost.
        /// </summary>
        public virtual bool IsFreeOfCost
        {
            get { return Price <= 0M; }
        }

        /// <summary>
        /// Gets or sets the data provider who handles this payment.
        /// </summary>
        public virtual PaymentHandlerModel PaymentHandler { get; set; }

        /// <summary>
        /// Gets or sets the data providers who can handle payments.
        /// </summary>
        public virtual IEnumerable<PaymentHandlerModel> PaymentHandlers { get; set; }

        #endregion
    }
}