using PhoneNumbers;
using store_api.Core.Exceptions;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace store_api.Core.Helpers
{
    public static class CommonHelper
    {
        private static PhoneNumberUtil _phoneUtil;
        private static Random _random;

        private static Random GetRandom()
        {
            return _random ?? new Random();
        }

        public static string MaskEmail(string email)
        {
            var _email = email.Split("@");
            var __email = _email[0][0] + MaskLength(_email[0].Length) + _email[0][_email[0].Length - 1] + "@" + _email[1];
            return __email;
        }

        public static string MaskLength(int length)
        {
            Random random = new Random();
            const string chars = "**********";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateSession()
        {
            return Guid.NewGuid().ToString();
        }

        public static string RandomNumber(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static DateTime Date()
        {
            return DateTime.UtcNow;
        }
        private static PhoneNumberUtil GetPhoneUtil()
        {
            if (_phoneUtil != null) return _phoneUtil; else { _phoneUtil = PhoneNumberUtil.GetInstance(); return _phoneUtil; }
        }
        public static bool IsPhoneNumberValid(string mobile, string country)
        {
            //+2348090678003
            try
            {
                if (mobile.Length != 14 && country == "NG") throw new CustomException("Invalid mobile number");
                var phone = GetPhoneUtil().Parse(mobile, "ZZ");
                if (phone == null) throw new CustomException("Cannot initialize mobile number");
                if (GetPhoneUtil().IsValidNumber(phone)) return true;
                return false;
            }
            catch (Exception ex)
            {
                if (ex is NumberParseException) throw new CustomException("The phone number spefified is not valid");
                throw;
            }
        }

        public static string IsPhoneNumberValid(string mobile)
        {
            try
            {
                var phone = GetPhoneUtil().Parse(mobile, "ZZ");
                if (phone == null) throw new CustomException("Cannot initialize mobile number");
                if (!GetPhoneUtil().IsValidNumber(phone)) throw new CustomException("Invalid mobile number entered");
                return "+" + phone.CountryCode;
            }
            catch (Exception ex)
            {
                if (ex is NumberParseException) throw new CustomException("The phone number spefified is not valid");
                throw;
            }
        }

        public static string GetCountryOfMobile(string mobile)
        {
            try
            {
                var phone = GetPhoneUtil().Parse(mobile, "ZZ");
                if (phone == null) throw new CustomException("Cannot initialize mobile number");
                if (!GetPhoneUtil().IsValidNumber(phone)) throw new CustomException("Invalid mobile number entered");
                return "+" + phone.CountryCode;
            }
            catch (Exception ex)
            {
                if (ex is NumberParseException) throw new CustomException("The phone number spefified is not valid");
                throw;
            }
        }

        public static string GenerateRef(string type)
        {
            switch (type)
            {
                case "Pay":
                    return "STORE-REF-" + Guid.NewGuid();
                default:
                    return "STORE-REF-" + Guid.NewGuid();
            };
        }

        public static bool IsStringValid(string value)
        {
            bool isValid = Regex.IsMatch(value, @"^[a-zA-Z0-9 ]+$", RegexOptions.IgnoreCase);
            return isValid;
        }

        public static bool IsValidString(string value)
        {
            bool isValid = Regex.IsMatch(value, @"^[a-zA-Z ]+$", RegexOptions.IgnoreCase);
            return isValid;
        }

        public static bool IsValidNumber(string value)
        {
            bool isValid = Regex.IsMatch(value, @"^[0-9]+$", RegexOptions.IgnoreCase);
            return isValid;
        }

        public static string Hash256(string randomString)
        {
            // SHA512Managed crypt = new SHA512Managed();
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString), 0, Encoding.ASCII.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

        public static string Sha256(string text)
        {
            byte[] value = Encoding.UTF8.GetBytes(text);
            var data = new SHA256Managed().ComputeHash(value);
            return ByteToString(data);
        }
        public static string ByteToString(byte[] buff)
        {
            string getbinary = "";
            for (int i = 0; i <= buff.Length - 1; i++)
            {
                getbinary += buff[i].ToString("X2");
            }
            return getbinary;
        }

        public static string GenerateRandomString()
        {
            return System.Guid.NewGuid().ToString().Replace("-", string.Empty).Replace("+", string.Empty).Substring(0, 6);
        }
        public static string CreateUniqueNumber()
        {
            return DateTime.UtcNow.Ticks.ToString().Substring(10);
        }
        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomStringOtp(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGJHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string HashData(string randomString)
        {
            // SHA512Managed crypt = new SHA512Managed();
            SHA256Managed crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString), 0, Encoding.ASCII.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }

    }
}
