using NUnit.Framework;

namespace RsaSecureToken.Tests
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        [Test]
        public void is_valid()
        {
            var target = new AuthenticationService(new FakeProfile(), new FakeToken());

            var actual = target.IsValid("joey", "91000000");

            //always failed
            Assert.IsTrue(actual);
        }
    }

    public class FakeToken : IToken
    {
        public string GetRandom(string account)
        {
            return "000000";
        }
    }

    public class FakeProfile : IProfile
    {
        public string GetPassword(string account)
        {
            return account == "joey" ? "91" : "";
        }
    }
}