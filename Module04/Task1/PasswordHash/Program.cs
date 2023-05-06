using System;
using System.Security.Cryptography;

namespace PasswordHash
{
    class Program
    {
        static void Main(string[] args)
        {
            string password = "myPassword123";
            byte[] salt = GenerateSalt();

            //Initial method version call
            //string hashedPassword = GeneratePasswordHashUsingSalt(password, salt);
            //Console.WriteLine($"Hashed Password: {hashedPassword}");

            string hashedPassword = GeneratePasswordHashUsingSaltImproved(password, salt);
            Console.WriteLine($"Hashed Password: {hashedPassword}");
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string GeneratePasswordHashUsingSaltImproved(string passwordText, byte[] salt)
        {
            const int saltSize = 16;  //Instead of separate hashBytes array with a fixed size of 36
            const int hashSize = 20;  //Also constants instead of hardcoded values - better for code readability and maintainability
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterations: 10000);
            byte[] hash = pbkdf2.GetBytes(hashSize);
            var passwordHashBytes = new byte[saltSize + hashSize];
            Buffer.BlockCopy(salt, 0, passwordHashBytes, 0, saltSize);          // More efficient copying the salt and hash into passwordHashBytes array
            Buffer.BlockCopy(hash, 0, passwordHashBytes, saltSize, hashSize);   // in a single operation, comeparing to Array.Copy
            var passwordHash = Convert.ToBase64String(passwordHashBytes);
            return passwordHash;
        }

        //Initial method version
        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var iterate = 10000;
            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }
        
    }
}
