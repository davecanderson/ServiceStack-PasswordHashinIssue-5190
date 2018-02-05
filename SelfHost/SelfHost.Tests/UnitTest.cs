using NUnit.Framework;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Testing;
using System.Security.Cryptography;

namespace SelfHost.Tests
{
    public class UnitTest
    {
        private readonly ServiceStackHost appHost;

        private string passwordHash = "lmXQNKAoWc+HXADy3U2yLd0lzactUDB1OqlUtd4PeugDJ06oKdVID82ellAbnShUPGhDibDW+SWU82sb702DqA==";
        private string salt = "aQWJVmQ=";

        public UnitTest()
        {
            appHost = new BasicAppHost().Init();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown() => appHost.Dispose();

        [Test]
        public void Verify_Password_SHA512Managed()
        {
            var isMatch = new SaltedHash(new SHA512Managed(), 5).VerifyHashString("123456", passwordHash, salt);
            Assert.That(isMatch);
        }

        [Test]
        public void Verify_Password_IHashProvider()
        {
            var isMatch = appHost.Resolve<IHashProvider>().VerifyHashString("123456", passwordHash, salt);
            Assert.That(isMatch);
        }
    }
}
