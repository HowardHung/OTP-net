using NSubstitute;
using NUnit.Framework;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _fakeProfile = Substitute.For<IProfile>();
            _token = Substitute.For<IToken>();
            _logger = Substitute.For<ILogger>();
            _target = new AuthenticationService(_fakeProfile, _token, _logger);
        }

        private IProfile _fakeProfile;
        private IToken _token;
        private AuthenticationService _target;
        private ILogger _logger;

        private void ShouldBeValid(string account, string password)
        {
            var actual = _target.IsValid(account, password);
            Assert.IsTrue(actual);
        }

        private void GivenToken(string returnThis)
        {
            _token.GetRandom("").ReturnsForAnyArgs(returnThis);
        }

        private void GivenPassword(string account, string returnThis)
        {
            _fakeProfile.GetPassword(account).Returns(returnThis);
        }

        private void ShouldBeInvalid(string account, string password)
        {
            var actual = _target.IsValid(account, password);
            Assert.IsFalse(actual);
        }

        [Test]
        public void is_invalid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            ShouldBeInvalid("joey", "wrong password");
        }

        [Test]
        public void is_valid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            ShouldBeValid("joey", "91000000");
        }

        [Test]
        public void log_when_invalid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            _target.IsValid("joey", "wrong password");
            _logger.Received(1).Save(Arg.Is<string>(x => x.Contains("joey")));
        }

        [Test]
        public void not_log_when_valid()
        {
            GivenPassword("joey", "91");
            GivenToken("000000");
            _target.IsValid("joey", "91000000");
            _logger.Received(0).Save(Arg.Any<string>());
        }
    }
}