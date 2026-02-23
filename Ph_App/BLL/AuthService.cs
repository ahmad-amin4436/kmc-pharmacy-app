using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ph_App.DAL;
using Ph_App.Models;
using Ph_App.Database;

namespace Ph_App.BLL
{
    public class AuthService
    {
        private static UserRepository _userRepository = PharmacyDBContext.Users;
        
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            // Simulate async
            return await Task.Run(() =>
            {
                var user = _userRepository.GetByUsername(username);
                if (user == null) return null;
                if (VerifyHash(password, user.PasswordHash)) return user;
                return null;
            });
        }
        
        // Simple PBKDF2 implementation
        public static string HashPassword(string password)
        {
            var salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider()) rng.GetBytes(salt);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }
        
        public static bool VerifyHash(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash)) return false;
            var hashBytes = Convert.FromBase64String(storedHash);
            //string haseh = CreateHashTest("123");
            if (hashBytes.Length != 36) return false;
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var hash = pbkdf2.GetBytes(20);
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i]) return false;
            return true;
        }
        public static string CreateHashTest(string password)
        {
            byte[] salt = new byte[16]; // all zeros (for testing only)

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
