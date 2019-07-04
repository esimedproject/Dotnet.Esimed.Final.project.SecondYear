using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Security.Cryptography;
using System.Reflection.Metadata;

namespace ApiMobile.Models
{
    public class Salt
    {
        public const int SaltByteSize = 16;
        public const int HashByteSize = 20;
        public const int HashingIterationsCount = 1000;

        internal static byte[] GenerateSalt()
        {
            using (RNGCryptoServiceProvider saltGenerator = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltByteSize];
                saltGenerator.GetBytes(salt);
                return salt;
            }
        }

        internal static byte[] ComputeHash(string password, byte[] salt)
        {
            using (Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt, HashingIterationsCount))
            {
                return hashGenerator.GetBytes(HashByteSize);
            }
        }
        internal static string GetPasswordHash(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] hash = ComputeHash(password, salt);
            byte[] hashBytes = new Byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);

        }
        internal static bool ComparePassword(string EmployePassword, string TextboxPassword)
        {
            if (!string.IsNullOrEmpty(EmployePassword))
            {
                byte[] hashBytes = Convert.FromBase64String(EmployePassword);
                byte[] salt = new byte[SaltByteSize];
                Array.Copy(hashBytes, 0, salt, 0, SaltByteSize);
                var pbkdf2 = new Rfc2898DeriveBytes(TextboxPassword, salt, HashingIterationsCount);
                byte[] hash = pbkdf2.GetBytes(HashByteSize);
                for (int i = 0; i < HashByteSize; i++)
                    if (hashBytes[i + SaltByteSize] != hash[i])
                    {
                        return false;
                    }
                return true;
            }
            return false;
        }
    }
}