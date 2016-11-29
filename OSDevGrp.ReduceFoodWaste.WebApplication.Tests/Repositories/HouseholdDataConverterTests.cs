using System;
using System.Linq;
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
        /// Tests that Convert converts HouseholdIdentificationView to HouseholdModel.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdIdentificationViewToHouseholdModel()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdIdentificationView = Fixture.Build<HouseholdIdentificationView>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(householdIdentificationView, Is.Not.Null);
            Assert.That(householdIdentificationView.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdIdentificationView.Name, Is.Not.Null);
            Assert.That(householdIdentificationView.Name, Is.Not.Empty);

            var result = householdDataConverter.Convert<HouseholdIdentificationView, HouseholdModel>(householdIdentificationView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(householdIdentificationView.HouseholdIdentifier));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(householdIdentificationView.Name));
            Assert.That(result.Description, Is.Null);
            Assert.That(result.PrivacyPolicy, Is.Null);
            Assert.That(result.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(result.HouseholdMembers, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdView to a HouseholdModel.
        /// </summary>
        [Test]
        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void TestThatConvertConvertsHouseholdViewToHouseholdModel(bool hasDesciption, bool hasHouseholdMembers)
        {
            HouseholdMemberIdentificationView[] householdMemberIdentificationViewCollection = null;
            if (hasHouseholdMembers)
            {
                var numberOfHouseholdMembers = Random.Next(5, 10);
                householdMemberIdentificationViewCollection = new HouseholdMemberIdentificationView[numberOfHouseholdMembers];
                for (var i = 0; i < numberOfHouseholdMembers; i++)
                {
                    var householdMemberIdentificationView = Fixture.Build<HouseholdMemberIdentificationView>()
                        .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                        .With(m => m.MailAddress, Fixture.Create<string>())
                        .With(m => m.ExtensionData, null)
                        .Create();
                    Assert.That(householdMemberIdentificationView, Is.Not.Null);
                    Assert.That(householdMemberIdentificationView.HouseholdMemberIdentifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Null);
                    Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Empty);

                    householdMemberIdentificationViewCollection[i] = householdMemberIdentificationView;
                }
            }

            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdView = Fixture.Build<HouseholdView>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, hasDesciption ? Fixture.Create<string>() : null)
                .With(m => m.CreationTime, DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.HouseholdMembers, householdMemberIdentificationViewCollection)
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(householdView, Is.Not.Null);
            Assert.That(householdView.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdView.Name, Is.Not.Null);
            Assert.That(householdView.Name, Is.Not.Empty);
            if (hasDesciption)
            {
                Assert.That(householdView.Description, Is.Not.Null);
                Assert.That(householdView.Description, Is.Not.Empty);
            }
            else
            {
                Assert.That(householdView.Description, Is.Null);
            }
            Assert.That(householdView.CreationTime, Is.Not.EqualTo(default(DateTime)));
            if (hasHouseholdMembers)
            {
                Assert.That(householdView.HouseholdMembers, Is.Not.Null);
                Assert.That(householdView.HouseholdMembers, Is.Not.Empty);
            }
            else
            {
                Assert.That(householdView.HouseholdMembers, Is.Null);
            }

            var result = householdDataConverter.Convert<HouseholdView, HouseholdModel>(householdView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(householdView.HouseholdIdentifier));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(householdView.Name));
            if (hasDesciption)
            {
                Assert.That(result.Description, Is.Not.Null);
                Assert.That(result.Description, Is.Not.Empty);
                Assert.That(result.Description, Is.EqualTo(householdView.Description));
            }
            else
            {
                Assert.That(result.Description, Is.Null);
            }
            Assert.That(result.PrivacyPolicy, Is.Null);
            Assert.That(result.CreationTime, Is.EqualTo(householdView.CreationTime));
            if (hasHouseholdMembers)
            {
                Assert.That(result.HouseholdMembers, Is.Not.Null);
                Assert.That(result.HouseholdMembers, Is.Not.Empty);
                Assert.That(result.HouseholdMembers.Count(), Is.EqualTo(householdView.HouseholdMembers.Length));
            }
            else
            {
                Assert.That(result.HouseholdMembers, Is.Null);
            }
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
                .With(m => m.HouseholdMembers, null)
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
        /// Tests that Convert converts a HouseholdModel to a HouseholdUpdateCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdModelToHouseholdUpdateCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.Identifier, Guid.NewGuid())
                .With(m => m.Name, Fixture.Create<string>())
                .With(m => m.Description, Fixture.Create<string>())
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);
            Assert.That(householdModel.Identifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdModel.Name, Is.Not.Null);
            Assert.That(householdModel.Name, Is.Not.Empty);
            Assert.That(householdModel.Description, Is.Not.Null);
            Assert.That(householdModel.Description, Is.Not.Empty);

            var result = householdDataConverter.Convert<HouseholdModel, HouseholdUpdateCommand>(householdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.HouseholdIdentifier, Is.EqualTo(householdModel.Identifier));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
            Assert.That(result.Name, Is.EqualTo(householdModel.Name));
            Assert.That(result.Description, Is.Not.Null);
            Assert.That(result.Description, Is.Not.Empty);
            Assert.That(result.Description, Is.EqualTo(householdModel.Description));
        }

        /// <summary>
        /// Tests that Convert converts a MemberOfHouseholdModel to a HouseholdAddHouseholdMemberCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsMemberOfHouseholdModelToHouseholdAddHouseholdMemberCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Empty);

            var result = householdDataConverter.Convert<MemberOfHouseholdModel, HouseholdAddHouseholdMemberCommand>(memberOfHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.HouseholdIdentifier, Is.EqualTo(memberOfHouseholdModel.HouseholdIdentifier));
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(memberOfHouseholdModel.MailAddress));
            Assert.That(result.TranslationInfoIdentifier, Is.EqualTo(default(Guid)));
        }

        /// <summary>
        /// Tests that Convert converts a MemberOfHouseholdModel where Removable is equal to true to a HouseholdRemoveHouseholdMemberCommand.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsMemberOfHouseholdModelWhereRemovableEqualTrueToHouseholdRemoveHouseholdMemberCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .With(m => m.Removable, true)
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Empty);
            Assert.That(memberOfHouseholdModel.Removable, Is.True);

            var result = householdDataConverter.Convert<MemberOfHouseholdModel, HouseholdRemoveHouseholdMemberCommand>(memberOfHouseholdModel);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.HouseholdIdentifier, Is.EqualTo(memberOfHouseholdModel.HouseholdIdentifier));
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(memberOfHouseholdModel.MailAddress));
        }

        /// <summary>
        /// Tests that Convert throws an ReduceFoodWasteSystemException when converting a MemberOfHouseholdModel where Removable is equal to false to a HouseholdRemoveHouseholdMemberCommand.
        /// </summary>
        [Test]
        public void TestThatConvertThrowsReduceFoodWasteSystemExceptionWhenConvertingMemberOfHouseholdModelWhereRemovableEqualTrueToHouseholdRemoveHouseholdMemberCommand()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .With(m => m.Removable, false)
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Null);
            Assert.That(memberOfHouseholdModel.MailAddress, Is.Not.Empty);
            Assert.That(memberOfHouseholdModel.Removable, Is.False);

            var exception = Assert.Throws<ReduceFoodWasteSystemException>(() => householdDataConverter.Convert<MemberOfHouseholdModel, HouseholdRemoveHouseholdMemberCommand>(memberOfHouseholdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Null);
            Assert.That(exception.Message, Is.Not.Empty);
            Assert.That(exception.Message, Is.EqualTo(Texts.CannotRemoveYourselfAsHouseholdMember));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdMemberIdentificationView to a HouseholdMemberModel.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdMemberIdentificationViewToHouseholdMemberModel()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdMemberIdentificationView = Fixture.Build<HouseholdMemberIdentificationView>()
                .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(householdMemberIdentificationView, Is.Not.Null);
            Assert.That(householdMemberIdentificationView.HouseholdMemberIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Null);
            Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Empty);

            var result = householdDataConverter.Convert<HouseholdMemberIdentificationView, HouseholdMemberModel>(householdMemberIdentificationView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(householdMemberIdentificationView.HouseholdMemberIdentifier));
            Assert.That(result.Name, Is.Null);
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(householdMemberIdentificationView.MailAddress));
            Assert.That(result.ActivationCode, Is.Null);
            Assert.That(result.IsActivated, Is.False);
            Assert.That(result.ActivatedTime, Is.Null);
            Assert.That(result.ActivatedTime.HasValue, Is.False);
            Assert.That(result.Membership, Is.Null);
            Assert.That(result.MembershipExpireTime, Is.Null);
            Assert.That(result.MembershipExpireTime.HasValue, Is.False);
            Assert.That(result.CanRenewMembership, Is.False);
            Assert.That(result.CanUpgradeMembership, Is.False);
            Assert.That(result.PrivacyPolicy, Is.Null);
            Assert.That(result.HasAcceptedPrivacyPolicy, Is.False);
            Assert.That(result.PrivacyPolicyAcceptedTime, Is.Null);
            Assert.That(result.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            Assert.That(result.HasReachedHouseholdLimit, Is.False);
            Assert.That(result.CreationTime, Is.EqualTo(default(DateTime)));
            Assert.That(result.UpgradeableMemberships, Is.Null);
            Assert.That(result.Households, Is.Null);
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdMemberIdentificationView to a MemberOfHouseholdModel.
        /// </summary>
        [Test]
        public void TestThatConvertConvertsHouseholdMemberIdentificationViewToMemberOfHouseholdModel()
        {
            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdMemberIdentificationView = Fixture.Build<HouseholdMemberIdentificationView>()
                .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(householdMemberIdentificationView, Is.Not.Null);
            Assert.That(householdMemberIdentificationView.HouseholdMemberIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Null);
            Assert.That(householdMemberIdentificationView.MailAddress, Is.Not.Empty);

            var result = householdDataConverter.Convert<HouseholdMemberIdentificationView, MemberOfHouseholdModel>(householdMemberIdentificationView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.HouseholdMemberIdentifier, Is.EqualTo(householdMemberIdentificationView.HouseholdMemberIdentifier));
            Assert.That(result.HouseholdIdentifier, Is.EqualTo(default(Guid)));
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(householdMemberIdentificationView.MailAddress));
            Assert.That(result.Removable, Is.False);
        }

        /// <summary>
        /// Tests that Convert converts a HouseholdMemberView to a HouseholdMemberModel.
        /// </summary>
        [Test]
        [TestCase(true, true, true, true, true, true, true)]
        [TestCase(true, true, true, true, true, true, false)]
        [TestCase(true, true, true, true, true, false, true)]
        [TestCase(true, true, true, true, true, false, false)]
        [TestCase(true, true, true, true, false, true, true)]
        [TestCase(true, true, true, true, false, true, false)]
        [TestCase(true, true, true, true, false, false, true)]
        [TestCase(true, true, true, true, false, false, false)]
        [TestCase(true, true, true, false, true, true, true)]
        [TestCase(true, true, true, false, true, true, false)]
        [TestCase(true, true, true, false, true, false, true)]
        [TestCase(true, true, true, false, true, false, false)]
        [TestCase(true, true, true, false, false, true, true)]
        [TestCase(true, true, true, false, false, true, false)]
        [TestCase(true, true, true, false, false, false, true)]
        [TestCase(true, true, true, false, false, false, false)]
        [TestCase(true, true, false, true, true, true, true)]
        [TestCase(true, true, false, true, true, true, false)]
        [TestCase(true, true, false, true, true, false, true)]
        [TestCase(true, true, false, true, true, false, false)]
        [TestCase(true, true, false, true, false, true, true)]
        [TestCase(true, true, false, true, false, true, false)]
        [TestCase(true, true, false, true, false, false, true)]
        [TestCase(true, true, false, true, false, false, false)]
        [TestCase(true, true, false, false, true, true, true)]
        [TestCase(true, true, false, false, true, true, false)]
        [TestCase(true, true, false, false, true, false, true)]
        [TestCase(true, true, false, false, true, false, false)]
        [TestCase(true, true, false, false, false, true, true)]
        [TestCase(true, true, false, false, false, true, false)]
        [TestCase(true, true, false, false, false, false, true)]
        [TestCase(true, true, false, false, false, false, false)]
        [TestCase(true, false, true, true, true, true, true)]
        [TestCase(true, false, true, true, true, true, false)]
        [TestCase(true, false, true, true, true, false, true)]
        [TestCase(true, false, true, true, true, false, false)]
        [TestCase(true, false, true, true, false, true, true)]
        [TestCase(true, false, true, true, false, true, false)]
        [TestCase(true, false, true, true, false, false, true)]
        [TestCase(true, false, true, true, false, false, false)]
        [TestCase(true, false, true, false, true, true, true)]
        [TestCase(true, false, true, false, true, true, false)]
        [TestCase(true, false, true, false, true, false, true)]
        [TestCase(true, false, true, false, true, false, false)]
        [TestCase(true, false, true, false, false, true, true)]
        [TestCase(true, false, true, false, false, true, false)]
        [TestCase(true, false, true, false, false, false, true)]
        [TestCase(true, false, true, false, false, false, false)]
        [TestCase(true, false, false, true, true, true, true)]
        [TestCase(true, false, false, true, true, true, false)]
        [TestCase(true, false, false, true, true, false, true)]
        [TestCase(true, false, false, true, true, false, false)]
        [TestCase(true, false, false, true, false, true, true)]
        [TestCase(true, false, false, true, false, true, false)]
        [TestCase(true, false, false, true, false, false, true)]
        [TestCase(true, false, false, true, false, false, false)]
        [TestCase(true, false, false, false, true, true, true)]
        [TestCase(true, false, false, false, true, true, false)]
        [TestCase(true, false, false, false, true, false, true)]
        [TestCase(true, false, false, false, true, false, false)]
        [TestCase(true, false, false, false, false, true, true)]
        [TestCase(true, false, false, false, false, true, false)]
        [TestCase(true, false, false, false, false, false, true)]
        [TestCase(true, false, false, false, false, false, false)]
        [TestCase(false, true, true, true, true, true, true)]
        [TestCase(false, true, true, true, true, true, false)]
        [TestCase(false, true, true, true, true, false, true)]
        [TestCase(false, true, true, true, true, false, false)]
        [TestCase(false, true, true, true, false, true, true)]
        [TestCase(false, true, true, true, false, true, false)]
        [TestCase(false, true, true, true, false, false, true)]
        [TestCase(false, true, true, true, false, false, false)]
        [TestCase(false, true, true, false, true, true, true)]
        [TestCase(false, true, true, false, true, true, false)]
        [TestCase(false, true, true, false, true, false, true)]
        [TestCase(false, true, true, false, true, false, false)]
        [TestCase(false, true, true, false, false, true, true)]
        [TestCase(false, true, true, false, false, true, false)]
        [TestCase(false, true, true, false, false, false, true)]
        [TestCase(false, true, true, false, false, false, false)]
        [TestCase(false, true, false, true, true, true, true)]
        [TestCase(false, true, false, true, true, true, false)]
        [TestCase(false, true, false, true, true, false, true)]
        [TestCase(false, true, false, true, true, false, false)]
        [TestCase(false, true, false, true, false, true, true)]
        [TestCase(false, true, false, true, false, true, false)]
        [TestCase(false, true, false, true, false, false, true)]
        [TestCase(false, true, false, true, false, false, false)]
        [TestCase(false, true, false, false, true, true, true)]
        [TestCase(false, true, false, false, true, true, false)]
        [TestCase(false, true, false, false, true, false, true)]
        [TestCase(false, true, false, false, true, false, false)]
        [TestCase(false, true, false, false, false, true, true)]
        [TestCase(false, true, false, false, false, true, false)]
        [TestCase(false, true, false, false, false, false, true)]
        [TestCase(false, true, false, false, false, false, false)]
        [TestCase(false, false, true, true, true, true, true)]
        [TestCase(false, false, true, true, true, true, false)]
        [TestCase(false, false, true, true, true, false, true)]
        [TestCase(false, false, true, true, true, false, false)]
        [TestCase(false, false, true, true, false, true, true)]
        [TestCase(false, false, true, true, false, true, false)]
        [TestCase(false, false, true, true, false, false, true)]
        [TestCase(false, false, true, true, false, false, false)]
        [TestCase(false, false, true, false, true, true, true)]
        [TestCase(false, false, true, false, true, true, false)]
        [TestCase(false, false, true, false, true, false, true)]
        [TestCase(false, false, true, false, true, false, false)]
        [TestCase(false, false, true, false, false, true, true)]
        [TestCase(false, false, true, false, false, true, false)]
        [TestCase(false, false, true, false, false, false, true)]
        [TestCase(false, false, true, false, false, false, false)]
        [TestCase(false, false, false, true, true, true, true)]
        [TestCase(false, false, false, true, true, true, false)]
        [TestCase(false, false, false, true, true, false, true)]
        [TestCase(false, false, false, true, true, false, false)]
        [TestCase(false, false, false, true, false, true, true)]
        [TestCase(false, false, false, true, false, true, false)]
        [TestCase(false, false, false, true, false, false, true)]
        [TestCase(false, false, false, true, false, false, false)]
        [TestCase(false, false, false, false, true, true, true)]
        [TestCase(false, false, false, false, true, true, false)]
        [TestCase(false, false, false, false, true, false, true)]
        [TestCase(false, false, false, false, true, false, false)]
        [TestCase(false, false, false, false, false, true, true)]
        [TestCase(false, false, false, false, false, true, false)]
        [TestCase(false, false, false, false, false, false, true)]
        [TestCase(false, false, false, false, false, false, false)]

        public void TestThatConvertConvertsHouseholdMemberViewToHouseholdMemberModel(bool isActivated, bool hasMembershipExpireTime, bool hasAcceptedPrivacyPolicy, bool hasHouseholds, bool canRenewMembership, bool canUpgradeMembership, bool hasReachedHouseholdLimit)
        {
            string[] upgradeableMemberships = Fixture.CreateMany<string>(Random.Next(1, 10)).ToArray();

            HouseholdIdentificationView[] householdIdentificationViewCollection = null;
            if (hasHouseholds)
            {
                var numberOfHouseholds = Random.Next(5, 10);
                householdIdentificationViewCollection = new HouseholdIdentificationView[numberOfHouseholds];
                for (var i = 0; i < numberOfHouseholds; i++)
                {
                    var householdIdentificationView = Fixture.Build<HouseholdIdentificationView>()
                        .With(m => m.HouseholdIdentifier, Guid.NewGuid())
                        .With(m => m.Name, Fixture.Create<string>())
                        .With(m => m.ExtensionData, null)
                        .Create();
                    Assert.That(householdIdentificationView, Is.Not.Null);
                    Assert.That(householdIdentificationView.HouseholdIdentifier, Is.Not.EqualTo(default(Guid)));
                    Assert.That(householdIdentificationView.Name, Is.Not.Null);
                    Assert.That(householdIdentificationView.Name, Is.Not.Empty);

                    householdIdentificationViewCollection[i] = householdIdentificationView;
                }
            }

            var householdDataConverter = CreateHouseholdDataConverter();
            Assert.That(householdDataConverter, Is.Not.Null);

            var householdMemberView = Fixture.Build<HouseholdMemberView>()
                .With(m => m.HouseholdMemberIdentifier, Guid.NewGuid())
                .With(m => m.MailAddress, Fixture.Create<string>())
                .With(m => m.IsActivated, isActivated)
                .With(m => m.ActivationTime, isActivated ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .With(m => m.Membership, Fixture.Create<string>())
                .With(m => m.MembershipExpireTime, hasMembershipExpireTime ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .With(m => m.CanRenewMembership, canRenewMembership)
                .With(m => m.CanUpgradeMembership, canUpgradeMembership)
                .With(m => m.IsPrivacyPolictyAccepted, hasAcceptedPrivacyPolicy)
                .With(m => m.PrivacyPolicyAcceptedTime, hasAcceptedPrivacyPolicy ? DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)) : (DateTime?) null)
                .With(m => m.HasReachedHouseholdLimit, hasReachedHouseholdLimit)
                .With(m => m.CreationTime, DateTime.Now.AddDays(Random.Next(7, 14)*-1).AddMinutes(Random.Next(-120, 120)))
                .With(m => m.UpgradeableMemberships, upgradeableMemberships)
                .With(m => m.Households, householdIdentificationViewCollection)
                .With(m => m.Payments, null)
                .With(m => m.ExtensionData, null)
                .Create();
            Assert.That(householdMemberView, Is.Not.Null);
            Assert.That(householdMemberView.HouseholdMemberIdentifier, Is.Not.EqualTo(default(Guid)));
            Assert.That(householdMemberView.MailAddress, Is.Not.Null);
            Assert.That(householdMemberView.MailAddress, Is.Not.Empty);
            Assert.That(householdMemberView.IsActivated, Is.EqualTo(isActivated));
            if (isActivated)
            {
                Assert.That(householdMemberView.ActivationTime, Is.Not.Null);
                Assert.That(householdMemberView.ActivationTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberView.ActivationTime, Is.Null);
                Assert.That(householdMemberView.ActivationTime.HasValue, Is.False);
            }
            Assert.That(householdMemberView.Membership, Is.Not.Null);
            Assert.That(householdMemberView.Membership, Is.Not.Empty);
            if (hasMembershipExpireTime)
            {
                Assert.That(householdMemberView.MembershipExpireTime, Is.Not.Null);
                Assert.That(householdMemberView.MembershipExpireTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberView.MembershipExpireTime, Is.Null);
                Assert.That(householdMemberView.MembershipExpireTime.HasValue, Is.False);
            }
            Assert.That(householdMemberView.CanRenewMembership, Is.EqualTo(canRenewMembership));
            Assert.That(householdMemberView.CanUpgradeMembership, Is.EqualTo(canUpgradeMembership));
            Assert.That(householdMemberView.IsPrivacyPolictyAccepted, Is.EqualTo(hasAcceptedPrivacyPolicy));
            if (hasAcceptedPrivacyPolicy)
            {
                Assert.That(householdMemberView.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(householdMemberView.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(householdMemberView.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(householdMemberView.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
            Assert.That(householdMemberView.HasReachedHouseholdLimit, Is.EqualTo(hasReachedHouseholdLimit));
            Assert.That(householdMemberView.CreationTime, Is.Not.EqualTo(default(DateTime)));
            Assert.That(householdMemberView.UpgradeableMemberships, Is.Not.Null);
            Assert.That(householdMemberView.UpgradeableMemberships, Is.Not.Empty);
            Assert.That(householdMemberView.UpgradeableMemberships, Is.EqualTo(upgradeableMemberships));
            if (hasHouseholds)
            {
                Assert.That(householdMemberView.Households, Is.Not.Null);
                Assert.That(householdMemberView.Households, Is.Not.Empty);
            }
            else
            {
                Assert.That(householdMemberView.Households, Is.Null);
            }
            Assert.That(householdMemberView.Payments, Is.Null);

            var result = householdDataConverter.Convert<HouseholdMemberView, HouseholdMemberModel>(householdMemberView);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Identifier, Is.EqualTo(householdMemberView.HouseholdMemberIdentifier));
            Assert.That(result.Name, Is.Null);
            Assert.That(result.MailAddress, Is.Not.Null);
            Assert.That(result.MailAddress, Is.Not.Empty);
            Assert.That(result.MailAddress, Is.EqualTo(householdMemberView.MailAddress));
            Assert.That(result.ActivationCode, Is.Null);
            Assert.That(result.IsActivated, Is.EqualTo(isActivated));
            if (isActivated)
            {
                Assert.That(result.ActivatedTime, Is.Not.Null);
                Assert.That(result.ActivatedTime, Is.EqualTo(householdMemberView.ActivationTime));
                Assert.That(result.ActivatedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(result.ActivatedTime, Is.Null);
                Assert.That(result.ActivatedTime.HasValue, Is.False);
            }
            Assert.That(result.Membership, Is.Not.Null);
            Assert.That(result.Membership, Is.Not.Empty);
            Assert.That(result.Membership, Is.EqualTo(householdMemberView.Membership));
            if (hasMembershipExpireTime)
            {
                Assert.That(result.MembershipExpireTime, Is.Not.Null);
                Assert.That(result.MembershipExpireTime, Is.EqualTo(householdMemberView.MembershipExpireTime));
                Assert.That(result.MembershipExpireTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(result.MembershipExpireTime, Is.Null);
                Assert.That(result.MembershipExpireTime.HasValue, Is.False);
            }
            Assert.That(result.CanRenewMembership, Is.EqualTo(canRenewMembership));
            Assert.That(result.CanUpgradeMembership, Is.EqualTo(canUpgradeMembership));
            Assert.That(result.PrivacyPolicy, Is.Null);
            Assert.That(result.HasAcceptedPrivacyPolicy, Is.EqualTo(hasAcceptedPrivacyPolicy));
            if (hasAcceptedPrivacyPolicy)
            {
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.Not.Null);
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.EqualTo(householdMemberView.PrivacyPolicyAcceptedTime));
                Assert.That(result.PrivacyPolicyAcceptedTime.HasValue, Is.True);
            }
            else
            {
                Assert.That(result.PrivacyPolicyAcceptedTime, Is.Null);
                Assert.That(result.PrivacyPolicyAcceptedTime.HasValue, Is.False);
            }
            Assert.That(result.HasReachedHouseholdLimit, Is.EqualTo(hasReachedHouseholdLimit));
            Assert.That(result.CreationTime, Is.EqualTo(householdMemberView.CreationTime));
            Assert.That(result.UpgradeableMemberships, Is.Not.Null);
            Assert.That(result.UpgradeableMemberships, Is.Not.Empty);
            Assert.That(result.UpgradeableMemberships.Count(), Is.EqualTo(upgradeableMemberships.Length));
            foreach (var upgradeableMembership in upgradeableMemberships)
            {
                Assert.That(result.UpgradeableMemberships.Contains(upgradeableMembership), Is.True);
            }
            if (hasHouseholds)
            {
                Assert.That(result.Households, Is.Not.Null);
                Assert.That(result.Households, Is.Not.Empty);
                Assert.That(result.Households.Count(), Is.EqualTo(householdMemberView.Households.Length));
            }
            else
            {
                Assert.That(result.Households, Is.Null);
            }
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
                .With(m => m.Households, null)
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
                .With(m => m.Households, null)
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
            Assert.That(result.AcceptedTime, Is.Null);
            Assert.That(result.AcceptedTime.HasValue, Is.False);
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
