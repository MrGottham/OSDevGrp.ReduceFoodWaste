using AutoFixture;
using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities
{
    /// <summary>
    /// Basic functionality for testing functionality.
    /// </summary>
    public abstract class TestBase
    {
        #region Private variables

        private static Fixture _fixture;
        private static Random _random;
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

        /// <summary>
        /// Returns a singleton instance of Ranodm.
        /// </summary>
        protected static Random Random
        {
            get
            {
                lock (SyncRoot)
                {
                    return _random ?? (_random = new Random(Fixture.Create<int>()));
                }
            }
        }

        #endregion
    }
}
