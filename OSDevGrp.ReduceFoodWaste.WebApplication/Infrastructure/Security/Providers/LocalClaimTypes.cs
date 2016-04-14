namespace OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Security.Providers
{
    /// <summary>
    /// Local claim types used by the Reduce Food Waste Web Application.
    /// </summary>
    public static class LocalClaimTypes
    {
        public const string CreatedHouseholdMember = "http://osdevgrp.local/foodwaste/security/createdhouseholdmember";
        public const string ActivatedHouseholdMember = "http://osdevgrp.local/foodwaste/security/activatedhouseholdmember";
        public const string PrivacyPoliciesAccepted = "http://osdevgrp.local/foodwaste/security/privacypoliciesaccepted";
        public const string ValidatedHouseholdMember = "http://osdevgrp.local/foodwaste/security/validatedhouseholdmember";
    }
}