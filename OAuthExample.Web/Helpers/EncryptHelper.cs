using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace OAuthExample.Web.Helpers
{
    public static class EncryptHelper
    {
        // 結構: [Nonce (12 bytes)] + [Encrypted Data (variable length)] + [Tag (16 bytes)]
        private const int TagSize = 16;
        private const int NonceSize = 12;

        public static string Encrypt(string plainText, string key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes = new byte[NonceSize + plainBytes.Length + TagSize];
            byte[] tag = new byte[TagSize];
            byte[] nonce = new byte[NonceSize];
            RandomNumberGenerator.Fill(nonce);

            using var aes = new AesGcm(Encoding.UTF8.GetBytes(key), TagSize);
            aes.Encrypt(nonce, plainBytes, cipherBytes.AsSpan(NonceSize, plainBytes.Length), tag);
            Buffer.BlockCopy(nonce, 0, cipherBytes, 0, NonceSize);
            Buffer.BlockCopy(tag, 0, cipherBytes, cipherBytes.Length - TagSize, TagSize);
            return Base64UrlEncoder.Encode(cipherBytes);
        }

        public static string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = Base64UrlEncoder.DecodeBytes(cipherText);
            byte[] plainBytes = new byte[cipherBytes.Length - NonceSize - TagSize];
            byte[] tag = new byte[TagSize];
            byte[] nonce = new byte[NonceSize];

            Buffer.BlockCopy(cipherBytes, 0, nonce, 0, NonceSize);
            Buffer.BlockCopy(cipherBytes, cipherBytes.Length - TagSize, tag, 0, TagSize);
            using var aes = new AesGcm(Encoding.UTF8.GetBytes(key), TagSize);
            aes.Decrypt(nonce, cipherBytes.AsSpan(NonceSize, plainBytes.Length), tag, plainBytes);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}
