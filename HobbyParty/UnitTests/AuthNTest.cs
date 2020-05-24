using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.AuthN;
using Services.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Constants = Services.Enums.Constants;

namespace UnitTests
{
    [TestClass]
    public class AuthNTest
    {


        [TestMethod]
        public void PasswordUnderMinimumLengthFailure()
        {
            var authService = new Authentication();
            //Always guarantees a length shorter than minimum
            char[] arr = new char[Constants.PasswordMinimumLength - 1];
            for(int i = 0; i < arr.Length; i++)
            {
                arr[i] = 'f';
            }
            var password = arr.ToString();

            Assert.IsFalse(authService.CheckPasswordStrength(password));
        }

        [TestMethod]
        public void PasswordMeetsRequirements()
        {
            var authService = new Authentication();
            var builder = new StringBuilder();
            while(builder.Length < Constants.PasswordMinimumLength)
            {
                builder.Append("CSULB2020");
            }

            Assert.IsTrue(authService.CheckPasswordStrength(builder.ToString()));
        }


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

        /// <summary>
        /// This method tests that two different inputs with the same salt will produce different hashes
        /// </summary>
        [TestMethod]
        public void SameSaltDifferentInputTest()
        {
            var authService = new Authentication();
            var tupleResult = authService.Hash("randomString", null);
            var tupleResult2 = authService.Hash("randomStringNumberTwo", tupleResult.Item2);
            Assert.AreNotEqual(tupleResult.Item1, tupleResult2.Item1);
            Assert.AreEqual(tupleResult.Item2, tupleResult2.Item2);
        }

        /// <summary>
        /// This method tests to see if the hashing algorithm can accept any special characters for utf8 encoding and such
        /// </summary>
        [TestMethod]
        public void SpecialCharactersHashing()
        {
            var authService = new Authentication();
            try
            {
                var tupleResult = authService.Hash("12346afl;kj,/'p...m[]\\-=+_-Ç¯àá¸2%", null);

            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
