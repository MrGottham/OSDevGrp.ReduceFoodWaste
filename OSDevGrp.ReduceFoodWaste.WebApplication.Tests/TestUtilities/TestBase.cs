using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities
{
    /// <summary>
    /// Basic functionality for testing functionality.
    /// </summary>
    public abstract class TestBase
    {
        #region Private variables

        private static Fixture _fixture;
        private static readonly object SyncRoot = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Returns a singleton instance of AutoFixture.
        /// </summary>
        protected static Fixture Fixture
        {
            get
            {
                lock (SyncRoot)
                {
                    return _fixture ?? (_fixture = new Fixture());
                }
            }
        }

        #endregion
    }
}
