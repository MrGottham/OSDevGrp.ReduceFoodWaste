using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

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
        /// Test that ToBase64 for a given type throws an ArgumentNullException when type is null.
        /// </summary>
        [Test]
        public void TestThatToBase64ForTypeThrowsArgumentNullExceptionWhenTypeIsNull()
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => modelHelper.ToBase64(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("type"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ToBase64 for a given type encodes and returns the given type's name.
        /// </summary>
        [Test]
        [TestCase(typeof(MembershipModel))]
        [TestCase(typeof(PayableModel))]
        [TestCase(typeof(PaymentHandlerModel))]
        [TestCase(typeof(DataProviderModel))]
        public void TestThatToBase64ForTypeEncodesTypeName(Type type)
        {
            string expectedValue = Convert.ToBase64String(Encoding.Default.GetBytes(type.FullName));
            Assert.That(expectedValue, Is.Not.Null);
            Assert.That(expectedValue, Is.Not.Empty);

            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            string result = modelHelper.ToBase64(type);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result, Is.EqualTo(expectedValue));
        }

        /// <summary>
        /// Test that ToType for a given encoded type name throws an ArgumentNullException when the encoded type name is invalid.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void TestThatToTypeForEncodedTypeNameThrowsArgumentNullExceptionWhenEncodedTypeNameIsInvalid(string invalidEncodedTypeName)
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => modelHelper.ToType(invalidEncodedTypeName));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("encodedTypeName"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ToType for a given encoded type name return the type fir the encoded type name is invalid.
        /// </summary>
        [Test]
        [TestCase(typeof(MembershipModel))]
        [TestCase(typeof(PayableModel))]
        [TestCase(typeof(PaymentHandlerModel))]
        [TestCase(typeof(DataProviderModel))]
        public void TestThatToTypeForEncodedTypeNameReturnsTypeForEncodedTypeName(Type type)
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            var encodedTypeName = modelHelper.ToBase64(type);
            Assert.That(encodedTypeName, Is.Not.Null);
            Assert.That(encodedTypeName, Is.Not.Empty);

            Type result = modelHelper.ToType(encodedTypeName);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(type));
        }

        /// <summary>
        /// Test that ToBase64 for a given model throws an ArgumentNullException when model is null.
        /// </summary>
        [Test]
        public void TestThatToBase64ForModelThrowsArgumentNullExceptionWhenModelIsNull()
        {
            IModelHelper modelHelper = CreateModelHelper();
            Assert.That(modelHelper, Is.Not.Null);

            ArgumentNullException exception =Assert.Throws<ArgumentNullException>(() => modelHelper.ToBase64((object) null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("model"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Test that ToBase64 for a given model serialize and returns the encoded value for a given model.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatToBase64ForModelSerializeAndReturnsEncodedValueForModel(string cultureName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.PriceCultureInfoName, cultureName)
                .Create();
            Assert.That(membershipModel, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Null);
            Assert.That(membershipModel.PriceCultureInfoName, Is.Not.Empty);
            Assert.That(membershipModel.PriceCultureInfoName, Is.EqualTo(cultureName));

            string expectedValue;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, membershipModel);

                memoryStream.Seek(0, SeekOrigin.Begin);

                expectedValue = Convert.ToBase64String(memoryStream.ToArray());
                Assert.That(expectedValue, Is.Not.Null);
                Assert.That(expectedValue, Is.Not.Empty);

                memoryStream.Close();
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
        /// Test that ToModel for a given encoded model deserialize and returns the model from a given encoded model.
        /// </summary>
        [Test]
        [TestCase("da-DK")]
        [TestCase("en-US")]
        [TestCase("es-ES")]
        public void TestThatToModelForEncodedModelDeserializeAndReturnsModelForEncodedModel(string cultureName)
        {
            MembershipModel membershipModel = Fixture.Build<MembershipModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.BillingInformation, Fixture.Create<string>())
                .With(m => m.Price, Fixture.Create<decimal>())
                .With(m => m.PriceCultureInfoName, cultureName)
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
