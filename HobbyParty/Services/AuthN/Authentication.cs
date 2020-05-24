using Services.Models;
using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Services.Enums;

namespace Services.AuthN
{
    public class Authentication
    {
      

        /// <summary>
        /// This method will register the user inside of the database 
        /// </summary>
        /// <param name="model">AccountRegistrationModel to register to Database, salt to store</param>
        /// <returns>Task wrapped bool with successCondition</returns>
        public async Task<bool> RegisterAccountAsync(AccountRegistrationModel model, string salt)
        {

            return false;
        }

        /// <summary>
        /// This method checks the strength of the password passed in to ensure it is of minimum requirements
        /// </summary>
        /// <param name="password">string to be checked</param>
        /// <returns>Boolean depending on if password is correct strength</returns>
        public bool CheckPasswordStrength(string password)
        {
            if(password.Length < Constants.PasswordMinimumLength)
                return false;
            if (!Constants.PasswordCharacterCheckRegex.IsMatch(password))
                return false;
            return true;
        }

        /// <summary>
        /// Hashes a value using a sha256 hash algo
        /// Will use the salt input if it is not null, otherwise it will generate a new salt completely 
        /// </summary>
        /// <param name="str">String to be hashed, possible string salt if login is attempted</param>
        /// <returns>A tuple containing a hashed string and its corresponding salt</returns>
        public Tuple<string, string> Hash(string str, string salt)
        {
            //Salty contains the actual salt value that will be used
            byte[] salty;

            //If Statement chekcs to see if string for salt is empty or null, and creates a new salt if it is 
            if(!string.IsNullOrEmpty(salt))
            {
                salty = Convert.FromBase64String(salt);
            }
            else
            {
                salty = new byte[Constants.SaltLength];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salty);
                }
            }
            //This line converts the passed in string to a hash that takes either the salt given or the salt generated. It takes iteration count from the constant file, and then performs an hmacsha256
            var hashedString = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: str,
                salt: salty,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Constants.HashIterations,
                numBytesRequested: 256 / 8)
                );

            return new Tuple<string, string>(hashedString, Convert.ToBase64String(salty));
        }


    }
}
