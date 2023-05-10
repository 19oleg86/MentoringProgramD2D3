using System;
using System.Linq;
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
            Console.ReadLine();
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
            const int hashSize = 20;  //Instead of hardcoded values - better for code readability and maintainability
            using (var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterations: 10000))
            {
                var passwordHashBytes = salt.Concat(pbkdf2.GetBytes(hashSize)).ToArray();
                return Convert.ToBase64String(passwordHashBytes);
            }
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
