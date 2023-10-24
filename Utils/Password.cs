using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace BTL_QuanLyBanDienThoai.Utils
{
    public class Password
    {
        public string HashPassword(string password, out string salt)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            salt = Convert.ToBase64String(saltBytes);

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8 
            ));

            return hashedPassword;
        }

        public bool VerifyPassword(string enteredPassword, string storedHashedPassword, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            byte[] enteredPasswordBytes = KeyDerivation.Pbkdf2(
                enteredPassword,
                saltBytes,
                KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000, 
                numBytesRequested: 256 / 8
            );

            string enteredPasswordHash = Convert.ToBase64String(enteredPasswordBytes);

            return enteredPasswordHash == storedHashedPassword;
        }
    }
}
