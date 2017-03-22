using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Models.Enums
{
    /// <summary>
    /// Paymetn status values for payable models.
    /// </summary>
    [Serializable]
    public enum PaymentStatus
    {
        Unpaid = 1,

        Paid = 2,

        Cancelled = 3
    }
}