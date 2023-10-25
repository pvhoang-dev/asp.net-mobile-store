using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BTL_QuanLyBanDienThoai.Utils
{
    public class Password
    {
        public  string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000, // You can adjust this
                numBytesRequested: 256 / 8 // 256 bits
            ));

            string saltBase64 = Convert.ToBase64String(salt);
            return $"{saltBase64}:{hashedPassword}";
        }

        public  bool VerifyPassword(string password, string storedHashedPassword)
        {
            var parts = storedHashedPassword.Split(':');
            if (parts.Length != 2)
            {
                return false; // Invalid format
            }

            string saltBase64 = parts[0];
            string hashedPassword = parts[1];

            byte[] salt = Convert.FromBase64String(saltBase64);

            string enteredHashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                salt,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return enteredHashedPassword == hashedPassword;
        }
    }
}
