using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BTL_QuanLyBanDienThoai.Utils
{
    public class Password
    {
        public string HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 32-byte hash
                string hashedPassword = Convert.ToBase64String(hash);
                return salt + ":" + hashedPassword; // Combine salt and hash
            }
        }


        public bool VerifyPassword(string enteredPassword, string storedHashedPassword)
        {
            string[] parts = storedHashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false; 
            }

            string salt = parts[0];
            string storedHash = parts[1];

            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32);
                string enteredHash = Convert.ToBase64String(hash);
                return storedHash == enteredHash;
            }
        }
    }
}
