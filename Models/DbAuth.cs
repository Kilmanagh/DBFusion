namespace DBFusion.Models
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class DbAuth
    {
        public string Username { get; set; }       // Database username
        public string Password { get; private set; }   // Encrypted password
        public string? ConnectionString { get; set; }  // Connection string
        public string? AuthToken { get; set; }         // Optional token-based authentication

        public DbAuth(string username, string password, string? connectionString, string? authToken = null)
        {
            Username = username ?? throw new ArgumentNullException(nameof(username), "Username cannot be null.");
            ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString), "Connection string cannot be null.");
            AuthToken = authToken;
            SetPassword(password); // Encrypt the password on initialization
        }

        // Encrypt and securely store the password
        private void SetPassword(string plainTextPassword)
        {
            Password = EncryptPassword(plainTextPassword);
        }

        // Retrieve the decrypted password
        public string GetDecryptedPassword()
        {
            return DecryptPassword(Password);
        }

        // Encrypt a password using AES encryption
        private string EncryptPassword(string plainText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("YourSecretKey12345"); // Replace with a secure key (16, 24, or 32 bytes)
                aes.IV = Encoding.UTF8.GetBytes("YourSecretIV12345");  // Replace with a secure IV (16 bytes)

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        // Decrypt a password using AES encryption
        private string DecryptPassword(string encryptedPassword)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes("YourSecretKey12345"); // Use the same key used for encryption
                aes.IV = Encoding.UTF8.GetBytes("YourSecretIV12345");  // Use the same IV used for encryption

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedPassword)))
                using (var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    var plainBytes = new byte[encryptedPassword.Length];
                    var count = cryptoStream.Read(plainBytes, 0, plainBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes, 0, count);
                }
            }
        }

        // Additional utility method to validate properties
        public void ValidateAuthDetails()
        {
            if (string.IsNullOrEmpty(Username))
                throw new InvalidOperationException("Username is required.");
            if (string.IsNullOrEmpty(ConnectionString))
                throw new InvalidOperationException("ConnectionString is required.");
        }
    }
}
