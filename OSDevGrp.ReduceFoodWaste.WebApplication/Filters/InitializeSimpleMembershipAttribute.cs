using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using WebMatrix.WebData;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Filters
{
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private class SimpleMembershipInitializer
        {
            #region Constructor

            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<UsersContext>(null);
                try
                {
                    using (var context = new UsersContext())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema.
                            ((IObjectContextAdapter) context).ObjectContext.CreateDatabase();
                        }
                    }

                    WebSecurity.InitializeDatabaseConnection("SecurityConnection", "UserProfile", "UserId", "UserName", true);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            #endregion
        }

        #region Private variables

        private static SimpleMembershipInitializer _simpleMembershipInitializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        #endregion

        #region Methods

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start.
            LazyInitializer.EnsureInitialized(ref _simpleMembershipInitializer, ref _isInitialized, ref _initializerLock);
        }

        #endregion
    }
}