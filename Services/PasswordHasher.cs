using System.Security.Cryptography;
using System.Text;

namespace RODRIGUEZ_MESSAGEBOXE.Services
{
    /// <summary>
    /// CONFIDENTIALITY: passwords are never stored or compared as plain text.
    /// Each password is combined with a unique, randomly generated salt and run through
    /// PBKDF2 (100,000 iterations, SHA-256, 256-bit key). The salt defeats precomputed
    /// "rainbow table" attacks, and the high iteration count makes brute-forcing slow even
    /// if the stored hash/salt pair were ever leaked.
    /// </summary>
    public static class PasswordHasher
    {
        private const int SaltSizeBytes = 16;   // 128-bit salt
        private const int KeySizeBytes = 32;    // 256-bit derived key
        private const int Iterations = 100_000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        /// <summary>
        /// Generates a new random salt and hashes the password with it.
        /// Returns the Base64-encoded hash and salt to be stored together.
        /// </summary>
        public static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSizeBytes);
            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                Algorithm,
                KeySizeBytes);

            return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
        }

        /// <summary>
        /// Re-hashes the supplied password with the stored salt and compares it to the
        /// stored hash using a constant-time comparison to avoid timing attacks.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);
            byte[] expectedHash = Convert.FromBase64String(storedHash);

            byte[] actualHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                Iterations,
                Algorithm,
                KeySizeBytes);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
