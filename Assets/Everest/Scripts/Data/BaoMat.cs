using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Everest {
    internal static class BaoMat {
        private static readonly ICryptoTransform encryptor;
        private static readonly ICryptoTransform decryptor;

        static BaoMat() {
            var key = Key.key;
            var vector = Key.vector;
            //Trộn thêm một bước nữa cho chắc ăn
            for (int i = 0; i < key.Length; i++) {
                key[i] = (byte)(key[i] * 5);
            }
            for (int i = 0; i < vector.Length; i++) {
                vector[i] = (byte)(vector[i] * 11);
            }
            using var rm = new RijndaelManaged { Padding = PaddingMode.Zeros };
            encryptor = rm.CreateEncryptor(key, vector);
            decryptor = rm.CreateDecryptor(key, vector);
        }

        //https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes

        internal static byte[] MaHoa(string plainText) {
            if (plainText == null || plainText.Length <= 0) throw new ArgumentNullException("plainText");
            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (StreamWriter swEncrypt = new(csEncrypt, Encoding.UTF8)) {
                swEncrypt.Write(plainText);
            }
            return msEncrypt.ToArray();
        }

        internal static string GiaiMa(byte[] cipherText) {
            if (cipherText == null || cipherText.Length <= 0) throw new ArgumentNullException("cipherText");
            using MemoryStream msDecrypt = new(cipherText);
            using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new(csDecrypt, Encoding.UTF8);
            return srDecrypt.ReadToEnd();
        }

        //https://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string

        internal static string GetMD5(string input) {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            for (int i = 0; i < hashBytes.Length; i++) {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
