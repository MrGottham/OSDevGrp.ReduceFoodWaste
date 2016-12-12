using System;
using System.Security.Principal;
using System.Threading;
using NUnit.Framework;
using OSDevGrp.ReduceFoodWaste.WebApplication.Models;
using OSDevGrp.ReduceFoodWaste.WebApplication.Repositories;
using OSDevGrp.ReduceFoodWaste.WebApplication.Tests.TestUtilities;
using Ploeh.AutoFixture;
using Rhino.Mocks;

namespace OSDevGrp.ReduceFoodWaste.WebApplication.Tests.Repositories
{
    /// <summary>
    /// Tests the repository which can access household data.
    /// </summary>
    [TestFixture]
    public class HouseholdDataRepositoryTests : TestBase
    {
        #region Private variables

        private ICredentialsProvider _credentialsProviderMock;
        private IHouseholdDataConverter _householdDataConverterMock;

        #endregion

        /// <summary>
        /// Initialize each test.
        /// </summary>
        [SetUp]
        public void TestInitialize()
        {
            _credentialsProviderMock = MockRepository.GenerateMock<ICredentialsProvider>();
            _householdDataConverterMock = MockRepository.GenerateMock<IHouseholdDataConverter>();
        }

        /// <summary>
        /// Tests that the constructor initialize a repository which can access household data.
        /// </summary>
        [Test]
        public void TestThatConstructorInitializeHouseholdDataRepository()
        {
            var householdDataRepository = new HouseholdDataRepository(_credentialsProviderMock, _householdDataConverterMock);
            Assert.That(householdDataRepository, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the provider which can creates credentials is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenCredentialsProviderIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(null, _householdDataConverterMock));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("credentialsProvider"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that the constructor throws an ArgumentNullException when the converter which can convert household data is null.
        /// </summary>
        [Test]
        public void TestThatConstructorThrowsArgumentNullExceptionWhenHouseholdDataConverterIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HouseholdDataRepository(_credentialsProviderMock, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdDataConverter"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsHouseholdMemberCreatedAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsHouseholdMemberCreatedAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.IsHouseholdMemberCreatedAsync(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that IsHouseholdMemberActivatedAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatIsHouseholdMemberActivatedAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.IsHouseholdMemberActivatedAsync(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that HasHouseholdMemberAcceptedPrivacyPolicyAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatHasHouseholdMemberAcceptedPrivacyPolicyAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.HasHouseholdMemberAcceptedPrivacyPolicyAsync(null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdMemberAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdMemberAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdMemberAsync(null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdMemberAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdMemberAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdMemberAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdIdentificationCollectionAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdIdentificationCollectionAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdIdentificationCollectionAsync(null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdIdentificationCollectionAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdIdentificationCollectionAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdIdentificationCollectionAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdAsync(null, householdModel, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdAsync throws an ArgumentNullException when the model for the household to get is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdAsyncThrowsArgumentNullExceptionWhenHouseholdModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetHouseholdAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetHouseholdAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), householdModel, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateHouseholdAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatCreateHouseholdAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.CreateHouseholdAsync(null, householdModel, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateHouseholdAsync throws an ArgumentNullException when the model for the household to crate is null.
        /// </summary>
        [Test]
        public void TestThatCreateHouseholdAsyncThrowsArgumentNullExceptionWhenHouseholdModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.CreateHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpdateHouseholdAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatUpdateHouseholdAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.UpdateHouseholdAsync(null, householdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that UpdateHouseholdAsync throws an ArgumentNullException when the model for the household to crate is null.
        /// </summary>
        [Test]
        public void TestThatUpdateHouseholdAsyncThrowsArgumentNullExceptionWhenHouseholdModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.UpdateHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddHouseholdMemberToHouseholdAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberToHouseholdAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.MailAddress, Fixture.Create<string>())
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.AddHouseholdMemberToHouseholdAsync(null, memberOfHouseholdModel, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddHouseholdMemberToHouseholdAsync throws an ArgumentNullException when the model for the household member to add is null.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberToHouseholdAsyncThrowsArgumentNullExceptionWhenMemberOfHouseholdModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.AddHouseholdMemberToHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("memberOfHouseholdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AddHouseholdMemberToHouseholdAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatAddHouseholdMemberToHouseholdAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.MailAddress, Fixture.Create<string>())
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.AddHouseholdMemberToHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), memberOfHouseholdModel, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveHouseholdMemberFromHouseholdAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatRemoveHouseholdMemberFromHouseholdAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var memberOfHouseholdModel = Fixture.Build<MemberOfHouseholdModel>()
                .With(m => m.MailAddress, Fixture.Create<string>())
                .Create();
            Assert.That(memberOfHouseholdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.RemoveHouseholdMemberFromHouseholdAsync(null, memberOfHouseholdModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that RemoveHouseholdMemberFromHouseholdAsync throws an ArgumentNullException when the model for the household member to add is null.
        /// </summary>
        [Test]
        public void TestThatRemoveHouseholdMemberFromHouseholdAsyncThrowsArgumentNullExceptionWhenMemberOfHouseholdModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.RemoveHouseholdMemberFromHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("memberOfHouseholdModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that CreateHouseholdAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatCreateHouseholdAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdModel = Fixture.Build<HouseholdModel>()
                .With(m => m.HouseholdMembers, null)
                .Create();
            Assert.That(householdModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.CreateHouseholdAsync(MockRepository.GenerateMock<IIdentity>(), householdModel, null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ActivateHouseholdMemberAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatActivateHouseholdMemberAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var householdMemberModel = Fixture.Build<HouseholdMemberModel>()
                .With(m => m.Households, null)
                .Create();
            Assert.That(householdMemberModel, Is.Not.Null);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.ActivateHouseholdMemberAsync(null, householdMemberModel));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that ActivateHouseholdMemberAsync throws an ArgumentNullException when the model for the household member which should be activated is null.
        /// </summary>
        [Test]
        public void TestThatActivateHouseholdMemberAsyncThrowsArgumentNullExceptionWhenHouseholdMemberModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.ActivateHouseholdMemberAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("householdMemberModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AcceptPrivacyPolicyAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatAcceptPrivacyPolicyAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.AcceptPrivacyPolicyAsync(null, Fixture.Create<PrivacyPolicyModel>()));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that AcceptPrivacyPolicyAsync throws an ArgumentNullException when the model for the household to crate is null.
        /// </summary>
        [Test]
        public void TestThatAcceptPrivacyPolicyAsyncThrowsArgumentNullExceptionWhenPrivacyPolicyModelIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.AcceptPrivacyPolicyAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("privacyPolicyModel"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetMembershipsAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetMembershipsAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetMembershipsAsync(null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetMembershipsAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetMembershipsAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetMembershipsAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetPrivacyPoliciesAsync throws an ArgumentNullException when the identity is null.
        /// </summary>
        [Test]
        public void TestThatGetPrivacyPoliciesAsyncThrowsArgumentNullExceptionWhenIdentityIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetPrivacyPoliciesAsync(null, Thread.CurrentThread.CurrentUICulture));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("identity"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Tests that GetPrivacyPoliciesAsync throws an ArgumentNullException when the culture informations which should be used for translation is null.
        /// </summary>
        [Test]
        public void TestThatGetPrivacyPoliciesAsyncThrowsArgumentNullExceptionWhenCultureInfoIsNull()
        {
            var householdDataRepository = CreateHouseholdDataRepository();
            Assert.IsNotNull(householdDataRepository);

            var exception = Assert.Throws<ArgumentNullException>(() => householdDataRepository.GetPrivacyPoliciesAsync(MockRepository.GenerateMock<IIdentity>(), null));
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Null);
            Assert.That(exception.ParamName, Is.Not.Empty);
            Assert.That(exception.ParamName, Is.EqualTo("cultureInfo"));
            Assert.That(exception.InnerException, Is.Null);
        }

        /// <summary>
        /// Creates a repository which can access household data for unit testing.
        /// </summary>
        /// <returns>Repository which can access household data for unit testing.</returns>
        private IHouseholdDataRepository CreateHouseholdDataRepository()
        {
            return new HouseholdDataRepository(_credentialsProviderMock, _householdDataConverterMock);
        }
    }
}
