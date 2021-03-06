﻿using AutoFixture;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Models
{
    /// <summary>
    /// Tests the model helper.
    /// </summary>
    [TestFixture]
    public class ModelHelperTests : TestBase
    {
        /// <summary>
        /// Test that the constructor initialize a model helper.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeModelHelper()
        {
            IModelHelper modelHelper = new ModelHelper();
            Assert.That(modelHelper, Is.Not.Null);
        }

        /// <summary>
        /// Test that ToBase64 for a given model throws an ArgumentNullException when model is null.
        /// </summary>
        [Test]
        public void TestThatToBase64ForModelThrowsArgumentNullExceptionWhenModelIsNull()
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            ArgumentNullException exception =Assert.Throws<ArgumentNullException>(() => modelHelper.ToBase64(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ToBase64 for a given model serialize and returns the base64 encoded value for a given model.
        /// </summary>
        [Test]
        [TestCase("da-DK", true)]
        [TestCase("en-US", true)]
        [TestCase("es-ES", true)]
        [TestCase("da-DK", false)]
        [TestCase("en-US", false)]
        [TestCase("es-ES", false)]
        public void TestThatToBase64ForModelSerializeAndReturnsBase64EncodedValueForModel(string cultureName, bool hasPaymentHandlerIdentifier)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.PriceCultureInfoName, cultureName)
                .With(m => m.PaymentHandlerIdentifier, hasPaymentHandlerIdentifier ? Guid.NewGuid() : (Guid?) null)
                .With(m => m.PaymentHandlers, Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            if (hasPaymentHandlerIdentifier)
            {
                Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Not.Null);
                Assert.That(membershipModel.PaymentHandlerIdentifier.HasValue, Is.True);
            }
            else
            {
                Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
                Assert.That(membershipModel.PaymentHandlerIdentifier.HasValue, Is.False);
            }
            Assert.That(membershipModel.PaymentHandlers, Is.Not.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Not.Empty);

            string expectedValue;
            using (MemoryStream serializeStream = new MemoryStream())
            {
                using (GZipStream gZipStream = new GZipStream(serializeStream, CompressionMode.Compress))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(gZipStream, membershipModel);
                }

                expectedValue = Convert.ToBase64String(serializeStream.ToArray());
                Assert.That(expectedValue, Is.Not.Null);
                Assert.That(expectedValue, Is.Not.Empty);

                serializeStream.Close();
            }

            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            string result = modelHelper.ToBase64(membershipModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Test that ToModel for a given encoded model throws an ArgumentNullException when the encoded model is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatToModelForEncodedModelThrowsArgumentNullExceptionWhenEncodedModelIsInvalid(string invalidEncodedModel)
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => modelHelper.ToModel(invalidEncodedModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("encodedModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ToModel for a given base64 encoded model deserialize and returns the model from a given encoded model.
        /// </summary>
        [Test]
        [TestCase("da-DK", true)]
        [TestCase("en-US", true)]
        [TestCase("es-ES", true)]
        [TestCase("da-DK", false)]
        [TestCase("en-US", false)]
        [TestCase("es-ES", false)]
        public void TestThatToModelForBase64EncodedModelDeserializeAndReturnsModelForEncodedModel(string cultureName, bool hasPaymentHandlerIdentifier)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.BillingInformation, Fixture.Create<string>())
                .With(m => m.Price, Fixture.Create<decimal>())
                .With(m => m.PriceCultureInfoName, cultureName)
                .With(m => m.PaymentHandlerIdentifier, hasPaymentHandlerIdentifier ? Guid.NewGuid() : (Guid?) null)
                .With(m => m.PaymentHandlers, Fixture.CreateMany<PaymentHandlerModel>(Random.Next(5, 10)).ToList())
                .With(m => m.CanRenew, Fixture.Create<bool>())
                .With(m => m.CanUpgrade, Fixture.Create<bool>())
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Null);
            Assert.That(membershipModel.Name, Is.Not.Empty);
            Assert.That(membershipModel.Description, Is.Not.Null);
            Assert.That(membershipModel.Description, Is.Not.Empty);
            Assert.That(membershipModel.BillingInformation, Is.Not.Null);
            Assert.That(membershipModel.BillingInformation, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(cultureName));
            Assert.That(membershipModel.PriceCultureInfo, Is.Not.Null);
            if (hasPaymentHandlerIdentifier)
            {
                Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Not.Null);
                Assert.That(membershipModel.PaymentHandlerIdentifier.HasValue, Is.True);
            }
            else
            {
                Assert.That(membershipModel.PaymentHandlerIdentifier, Is.Null);
                Assert.That(membershipModel.PaymentHandlerIdentifier.HasValue, Is.False);
            }
            Assert.That(membershipModel.PaymentHandlers, Is.Not.Null);
            Assert.That(membershipModel.PaymentHandlers, Is.Not.Empty);

            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            string encodedModel = modelHelper.ToBase64(membershipModel);
            Assert.That(encodedModel, Is.Not.Null);
            Assert.That(encodedModel, Is.Not.Empty);

            object result = modelHelper.ToModel(encodedModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<MembershipModel>());

            MembershipModel clone = (MembershipModel) result;
            Assert.That(clone, Is.Not.Null);
            Assert.That(clone.Name, Is.Not.Null);
            Assert.That(clone.Name, Is.Not.Empty);
            Assert.That(clone.Name, Is.EqualTo(membershipModel.Name));
            Assert.That(clone.Description, Is.Not.Null);
            Assert.That(clone.Description, Is.Not.Empty);
            Assert.That(clone.Description, Is.EqualTo(membershipModel.Description));
            Assert.That(clone.BillingInformation, Is.Not.Null);
            Assert.That(clone.BillingInformation, Is.Not.Empty);
            Assert.That(clone.BillingInformation, Is.EqualTo(membershipModel.BillingInformation));
            Assert.That(clone.Price, Is.EqualTo(membershipModel.Price));
            Assert.That(clone.PriceCultureInfoName, Is.Not.Null);
            Assert.That(clone.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(clone.PriceCultureInfoName, Is.EqualTo(membershipModel.PriceCultureInfoName));
            Assert.That(clone.PriceCultureInfo, Is.Not.Null);
            Assert.That(clone.PriceCultureInfo, Is.EqualTo(membershipModel.PriceCultureInfo));
            if (hasPaymentHandlerIdentifier)
            {
                Assert.That(clone.PaymentHandlerIdentifier, Is.Not.Null);
                Assert.That(clone.PaymentHandlerIdentifier.HasValue, Is.True);
                // ReSharper disable PossibleInvalidOperationException
                Assert.That(clone.PaymentHandlerIdentifier.Value, Is.EqualTo(membershipModel.PaymentHandlerIdentifier.Value));
                // ReSharper restore PossibleInvalidOperationException
            }
            else
            {
                Assert.That(clone.PaymentHandlerIdentifier, Is.Null);
                Assert.That(clone.PaymentHandlerIdentifier.HasValue, Is.False);
            }
            Assert.That(clone.PaymentHandlers, Is.Not.Null);
            Assert.That(clone.PaymentHandlers, Is.Not.Empty);
            Assert.That(clone.PaymentHandlers.Count(), Is.EqualTo(membershipModel.PaymentHandlers.Count()));
            for (var i = 0; i < membershipModel.PaymentHandlers.Count(); i++)
            {
                Assert.That(clone.PaymentHandlers.ElementAt(i), Is.Not.Null);
                Assert.That(clone.PaymentHandlers.ElementAt(i).Identifier, Is.EqualTo(membershipModel.PaymentHandlers.ElementAt(i).Identifier));
                Assert.That(clone.PaymentHandlers.ElementAt(i).Name, Is.Not.Null);
                Assert.That(clone.PaymentHandlers.ElementAt(i).Name, Is.Not.Empty);
                Assert.That(clone.PaymentHandlers.ElementAt(i).Name, Is.EqualTo(membershipModel.PaymentHandlers.ElementAt(i).Name));
                Assert.That(clone.PaymentHandlers.ElementAt(i).DataSourceStatement, Is.Not.Null);
                Assert.That(clone.PaymentHandlers.ElementAt(i).DataSourceStatement, Is.Not.Empty);
                Assert.That(clone.PaymentHandlers.ElementAt(i).DataSourceStatement, Is.EqualTo(membershipModel.PaymentHandlers.ElementAt(i).DataSourceStatement));
            }
            Assert.That(clone.CanRenew, Is.EqualTo(membershipModel.CanRenew));
            Assert.That(clone.CanUpgrade, Is.EqualTo(membershipModel.CanUpgrade));
        }

        /// <summary>
        /// Creates a model helper for unit testing.
        /// </summary>
        /// <returns>Model helper for unit testing.</returns>
        private IModelHelper CreateModelHelper()
        {
            return new ModelHelper();
        }
    }
}
