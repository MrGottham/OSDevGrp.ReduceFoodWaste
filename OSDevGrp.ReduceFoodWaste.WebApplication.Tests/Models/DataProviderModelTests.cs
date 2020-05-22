using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model for a data provider who provides data.
    /// </summary>
    [TestFixture]
    public class DataProviderModelTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a model for a data provider who provides data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeDataProviderModel()
        {
            var dataProviderModel = new DataProviderModel();
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.Identifier, Is.EqualTo(default(Guid)));
            Assert.That(dataProviderModel.Name, Is.Null);
            Assert.That(dataProviderModel.DataSourceStatement, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for Identifier sets a new value.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValue()
        {
            var dataProviderModel = new DataProviderModel();
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.Identifier, Is.EqualTo(default(Guid)));

            var newValue = Guid.NewGuid();
            Assert.That(newValue, Is.Not.EqualTo(default(Guid)));

            dataProviderModel.Identifier = newValue;
            Assert.That(dataProviderModel.Identifier, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Identifier sets the value to the default for a Guid.
        /// </summary>
        [Test]
        public void TestThatIdentifierSetterSetsValueToDefaultForGuid()
        {
            var dataProviderModel = new DataProviderModel
            {
                Identifier = Guid.NewGuid()
            };
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.Identifier, Is.Not.EqualTo(default(Guid)));

            dataProviderModel.Identifier = default(Guid);
            Assert.That(dataProviderModel.Identifier, Is.EqualTo(default(Guid)));
        }

        /// <summary>
        /// Tests that the setter for Name sets a new value.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValue()
        {
            var dataProviderModel = new DataProviderModel();
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.Name, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            dataProviderModel.Name = newValue;
            Assert.That(dataProviderModel.Name, Is.Not.Null);
            Assert.That(dataProviderModel.Name, Is.Not.Empty);
            Assert.That(dataProviderModel.Name, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for Name sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatNameSetterSetsValueToNull()
        {
            var dataProviderModel = new DataProviderModel
            {
                Name = Fixture.Create<string>()
            };
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.Name, Is.Not.Null);
            Assert.That(dataProviderModel.Name, Is.Not.Empty);

            dataProviderModel.Name = null;
            Assert.That(dataProviderModel.Name, Is.Null);
        }

        /// <summary>
        /// Tests that the setter for DataSourceStatement sets a new value.
        /// </summary>
        [Test]
        public void TestThatDataSourceStatementSetterSetsValue()
        {
            var dataProviderModel = new DataProviderModel();
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.DataSourceStatement, Is.Null);

            var newValue = Fixture.Create<string>();
            Assert.That(newValue, Is.Not.Null);
            Assert.That(newValue, Is.Not.Empty);

            dataProviderModel.DataSourceStatement = newValue;
            Assert.That(dataProviderModel.DataSourceStatement, Is.Not.Null);
            Assert.That(dataProviderModel.DataSourceStatement, Is.Not.Empty);
            Assert.That(dataProviderModel.DataSourceStatement, Is.EqualTo(newValue));
        }

        /// <summary>
        /// Tests that the setter for DataSourceStatement sets the value to NULL.
        /// </summary>
        [Test]
        public void TestThatDataSourceStatementSetterSetsValueToNull()
        {
            var dataProviderModel = new DataProviderModel
            {
                DataSourceStatement = Fixture.Create<string>()
            };
            Assert.That(dataProviderModel, Is.Not.Null);
            Assert.That(dataProviderModel.DataSourceStatement, Is.Not.Null);
            Assert.That(dataProviderModel.DataSourceStatement, Is.Not.Empty);

            dataProviderModel.DataSourceStatement = null;
            Assert.That(dataProviderModel.DataSourceStatement, Is.Null);
        }
    }
}
