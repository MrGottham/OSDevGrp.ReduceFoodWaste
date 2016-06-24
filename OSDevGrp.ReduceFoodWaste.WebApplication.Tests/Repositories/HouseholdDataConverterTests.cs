using System;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.HouseholdDataService;
using OSDevGrp.ReduceFoodWaste.WebApplication.Infrastructure.Exceptions;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Resources;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the converter which can convert household data.
    /// </summary>
    [TestFixture]
    public class HouseholdDataConverterTests : TestBase
    {
        /// <summary>
        /// Tests that the constructor initialize a converter which can convert household data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataConverter()
        {
            var householdDataConverter = new HouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Convert throws an ArgumentNullException when the source object is null.
        /// </summary>
        [Test]
        public void TestThatConvertThrowsArgumentNullExceptionWhenSourceObjectIsNull()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataConverter.Convert<object, object>(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("source"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdModel to a HouseholdAddCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdModelToHouseholdAddCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Name, Is.Not.Null);
            Assert.That(householdModel.Name, Is.Not.Empty);
            Assert.That(householdModel.Description, Is.Not.Null);
            Assert.That(householdModel.Description, Is.Not.Empty);

            var result = householdDataConverter.Convert<HouseholdModel, HouseholdAddCommand>(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(householdModel.Name));
            Assert.That(result.Description, Is.Not.Null);
            Assert.That(result.Description, Is.Not.Empty);
            Assert.That(result.Description, Is.EqualTo(householdModel.Description));
            Assert.That(result.TranslationInfoIdentifier, Is.EqualTo(default(Guid)));
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdMemberModel with an activation code to a HouseholdMemberActivateCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdMemberModelWithActivationCodeToHouseholdMemberActivateCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var activationCode = Fixture.Create<string>();
            Assert.That(activationCode, Is.Not.Null);
            Assert.That(activationCode, Is.Not.Empty);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivationCode, activationCode)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.Not.Empty);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(activationCode));

            var result = householdDataConverter.Convert<HouseholdMemberModel, HouseholdMemberActivateCommand>(householdMemberModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ActivationCode, Is.Not.Null);
            Assert.That(result.ActivationCode, Is.Not.Empty);
            Assert.That(result.ActivationCode, Is.EqualTo(activationCode));
        }

        /// <summary>
        /// Tests that Convert throws an ReduceFoodWasteSystemException when converting a HouseholdMemberModel without an activation code to a HouseholdMemberActivateCommand.
        /// </summary>
        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("  ")]
        [TestCase("   ")]
        public void TestThatConvertThrowsReduceFoodWasteSystemExceptionWhenConvertinHouseholdMemberModelWithoutActivationCodeToHouseholdMemberActivateCommand(string illegalActivationCode)
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.ActivationCode, illegalActivationCode)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);
            Assert.That(householdMemberModel.ActivationCode, Is.EqualTo(illegalActivationCode));

            var exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdDataConverter.Convert<HouseholdMemberModel, HouseholdMemberActivateCommand>(householdMemberModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.ActivationCodeMustBeGiven));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a PrivacyPolicyModel where IsAccepted is equal to true to a HouseholdMemberAcceptPrivacyPolicyCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsPrivacyPolicyModelWhereIsAcceptedEqualToTrueToHouseholdMemberAcceptPrivacyPolicyCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, true)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.True);

            var result = householdDataConverter.Convert<PrivacyPolicyModel, HouseholdMemberAcceptPrivacyPolicyCommand>(privacyPolicyModel);
            Assert.That(result, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Convert throws an ReduceFoodWasteSystemException when converting a PrivacyPolicyModel where IsAccepted is equal to false to a HouseholdMemberAcceptPrivacyPolicyCommand.
        /// </summary>
        [Test]
        public void TestThatConvertThrowsReduceFoodWasteSystemExceptionWhenConvertingPrivacyPolicyModelWhereIsAcceptedEqualToFalseToHouseholdMemberAcceptPrivacyPolicyCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var privacyPolicyModel = Fixture.Build<PrivacyPolicyModel>()
                .With(m => m.IsAccepted, false)
                .Create();
            Assert.That(privacyPolicyModel, Is.Not.Null);
            Assert.That(privacyPolicyModel.IsAccepted, Is.False);

            var exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdDataConverter.Convert<PrivacyPolicyModel, HouseholdMemberAcceptPrivacyPolicyCommand>(privacyPolicyModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.PrivacyPoliciesHasNotBeenAccepted));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a BooleanResult to a Boolean.
        /// </summary>
        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestThatConvertConvertsBooleanResultToBoolean(bool testValue)
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var booleanResult = Fixture.Build<BooleanResult>()
                .With(m => m.Result, testValue)
                .With(m => m.EventDate, DateTime.Now)
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(booleanResult, Is.Not.Null);
            Assert.That(booleanResult.Result, Is.EqualTo(testValue));

            var result = householdDataConverter.Convert<BooleanResult, bool>(booleanResult);
            Assert.That(result, Is.EqualTo(testValue));
        }

        /// <summary>
        /// Tests that Convert converts a StaticTextView to a PrivacyPolicyModel.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsStaticTextViewToPrivacyPolicyModel()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var staticTextView = Fixture.Build<StaticTextView>()
                .With(m => m.StaticTextIdentifier, Guid.NewGuid())
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(staticTextView, Is.Not.Null);
            Assert.That(staticTextView.StaticTextIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(staticTextView.SubjectTranslation, Is.Not.Null);
            Assert.That(staticTextView.SubjectTranslation, Is.Not.Empty);
            Assert.That(staticTextView.BodyTranslation, Is.Not.Null);
            Assert.That(staticTextView.BodyTranslation, Is.Not.Empty);

            var result = householdDataConverter.Convert<StaticTextView, PrivacyPolicyModel>(staticTextView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(staticTextView.StaticTextIdentifier));
            Assert.That(result.Header, Is.Not.Null);
            Assert.That(result.Header, Is.Not.Empty);
            Assert.That(result.Header, Is.EqualTo(staticTextView.SubjectTranslation));
            Assert.That(result.Body, Is.Not.Null);
            Assert.That(result.Body, Is.Not.Empty);
            Assert.That(result.Body, Is.EqualTo(staticTextView.BodyTranslation));
            Assert.That(result.IsAccepted, Is.False);
        }

        /// <summary>
        /// Tests that Convert removes the HTML tag from the body when converting a StaticTextView to a PrivacyPolicyModel.
        /// </summary>
        [Test]
        public void TestThatConvertRemovesHtmlTagFromBodyWhenConvertingStaticTextViewToPrivacyPolicyModel()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var staticTextView = Fixture.Build<StaticTextView>()
                .With(m => m.StaticTextIdentifier, Guid.NewGuid())
                .With(m => m.BodyTranslation, string.Format("<html>{0}</html>", Fixture.Create<string>()))
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(staticTextView, Is.Not.Null);
            Assert.That(staticTextView.BodyTranslation, Is.Not.Null);
            Assert.That(staticTextView.BodyTranslation, Is.Not.Empty);
            Assert.That(staticTextView.BodyTranslation.Contains("<html>"), Is.True);
            Assert.That(staticTextView.BodyTranslation.Contains("</html>"), Is.True);

            var result = householdDataConverter.Convert<StaticTextView, PrivacyPolicyModel>(staticTextView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Body, Is.Not.Null);
            Assert.That(result.Body, Is.Not.Empty);
            Assert.That(result.Body.Contains("<html>"), Is.False);
            Assert.That(result.Body.Contains("</html>"), Is.False);
        }

        /// <summary>
        /// Creates a converter which can convert household data for unit testing.
        /// </summary>
        /// <returns>Converter which can convert household data for unit testing.</returns>
        private static IHouseholdDataConverter CreateHouseholdDataConverter()
        {
            return new HouseholdDataConverter();
        }
    }
}
