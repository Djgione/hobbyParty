using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.AuthN;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace UnitTests
{
    [TestClass]
    public class AuthNTest
    {
        /// <summary>
        /// This method tests to see if a hash and salt are created when no salt is passed in
        /// </summary>
        [TestMethod]
        public void HashCreationNoSaltTest()
        {
            var authNService = new Authentication();
            var tupleResult = authNService.Hash("randomString", null);

            Assert.IsFalse(string.IsNullOrEmpty(tupleResult.Item1));
            Assert.IsFalse(string.IsNullOrEmpty(tupleResult.Item2));
        }

        /// <summary>
        /// This method tests to see if it returns the same salt it passes, thus using it inside the hash
        /// </summary>
        [TestMethod]
        public void HashCreationWithSaltTest()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            var authNService = new Authentication();
            var tupleResult = authNService.Hash("randomString", Convert.ToBase64String(salt));
            Assert.AreEqual(Convert.ToBase64String(salt), tupleResult.Item2);
        }


        /// <summary>
        /// This method tests to see if a salt created from the method can be input back in to generate the same exact hash value when using same input
        /// </summary>
        [TestMethod]
        public void SameHashSameKeyTest()
        {
            var authNService = new Authentication();
            var tupleResult = authNService.Hash("randomString", null);
            var tupleResultSameSalt = authNService.Hash("randomString", tupleResult.Item2);
            Assert.AreEqual(tupleResult, tupleResultSameSalt);
        }

    }
}
