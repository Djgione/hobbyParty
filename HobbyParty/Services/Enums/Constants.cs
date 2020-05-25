using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Services.Enums
{
    public class Constants
    {
        public static readonly string DateStringPattern = "yyyy.MM.dd HH:mm:ss.fff";
        public static readonly int PasswordMinimumLength = 8;
        public static readonly Regex PasswordCharacterCheckRegex = new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-+_!@#$%^&*.,?])");
        public static readonly int HashIterations = 10000;
        public static readonly int SaltLength = 256 / 8;
    }
}
