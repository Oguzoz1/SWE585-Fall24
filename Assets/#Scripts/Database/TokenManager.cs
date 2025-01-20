using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Database
{
    public static class TokenManager
    {
        private static string _userToken;
        public static void SetUserToken(string token)
        {
            _userToken = token;

            string encryptedToken = Encrypt(token);
            PlayerPrefs.SetString("authToken", encryptedToken);
            PlayerPrefs.Save();

            Debug.Log("TOKEN IS SET");

        }
        public static void ClearToken()
        {
            PlayerPrefs.DeleteKey("authToken");
        }

        public static string GetToken()
        {
            string encryptedToken = PlayerPrefs.GetString("authToken", "");
            return string.IsNullOrEmpty(encryptedToken) ? null : Decrypt(encryptedToken);
        }
        private static string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                // Ensure key is 256 bits (32 bytes)
                aes.Key = Encoding.UTF8.GetBytes(_userToken.PadRight(32).Substring(0, 32)); // 256-bit key (padded or truncated)

                // Generate a random IV for each encryption
                aes.GenerateIV(); // Generates a new IV for each encryption
                byte[] iv = aes.IV;

                aes.Mode = CipherMode.CBC; // Set to CBC mode for better security
                aes.Padding = PaddingMode.PKCS7; // Use PKCS7 padding for better compatibility

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

                    // Combine the IV and encrypted data to send them together
                    byte[] result = new byte[iv.Length + encrypted.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);

                    return Convert.ToBase64String(result); // Return the IV and ciphertext as a Base64 string
                }
            }
        }

        private static string Decrypt(string encryptedText)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);

            // Extract the IV (first 16 bytes) and ciphertext (remaining bytes)
            byte[] iv = new byte[16];
            byte[] ciphertext = new byte[bytes.Length - 16];
            Array.Copy(bytes, 0, iv, 0, iv.Length);
            Array.Copy(bytes, iv.Length, ciphertext, 0, ciphertext.Length);

            using (Aes aes = Aes.Create())
            {
                // Ensure key is 256 bits (32 bytes)
                aes.Key = Encoding.UTF8.GetBytes(_userToken.PadRight(32).Substring(0, 32)); // 256-bit key (padded or truncated)
                aes.IV = iv; // Set the IV from the encrypted data

                aes.Mode = CipherMode.CBC; // Set to CBC mode for decryption
                aes.Padding = PaddingMode.PKCS7; // Use PKCS7 padding

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }



    }
}
