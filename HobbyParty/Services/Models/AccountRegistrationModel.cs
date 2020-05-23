using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class AccountRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public AccountRegistrationModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
