using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class Encryption : IEncryption
    {
        public string Decrypt(string stringToDecrypt)
        {
            try
            {
                if (string.IsNullOrEmpty(stringToDecrypt))
                    return string.Empty;

                SymmetricAlgorithm myAlg = Aes.Create();
                byte[] saltValueBytes = Encoding.Unicode.GetBytes("Zone 2 Tournament");
                PasswordDeriveBytes passwordKey = new("TheNat10nsPr1de", saltValueBytes, "SHA1", 3);
                myAlg.Key = passwordKey.GetBytes(myAlg.KeySize / 8);
                myAlg.IV = passwordKey.GetBytes(myAlg.BlockSize / 8);

                byte[] encrypted = Convert.FromBase64String(stringToDecrypt);
                using MemoryStream msDecrypt = new(encrypted);
                using CryptoStream csDecrypt = new(msDecrypt, myAlg.CreateDecryptor(), CryptoStreamMode.Read);
                using StreamReader sr = new(csDecrypt, Encoding.Unicode);
                string decrypted = sr.ReadToEnd();
                return decrypted;
            }
            catch (CryptographicException)
            {
                return string.Empty;
            }
            catch (FormatException)
            {
                return string.Empty;
            }
        }

        public string DecryptFromFile(string fileFullName)
        {
            try
            {
                string fileString = File.ReadAllText(fileFullName);
                return Decrypt(fileString);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
        }

        public string Encrypt(string stringToEncrypt)
        {
            SymmetricAlgorithm myAlg = Aes.Create();

            byte[] saltValueBytes = Encoding.Unicode.GetBytes("Zone 2 Tournament");
            PasswordDeriveBytes passwordKey = new("TheNat10nsPr1de", saltValueBytes, "SHA1", 3);
            myAlg.Key = passwordKey.GetBytes(myAlg.KeySize / 8);
            myAlg.IV = passwordKey.GetBytes(myAlg.BlockSize / 8);

            using MemoryStream msEncrypt = new();
            using CryptoStream csEncrypt = new(msEncrypt, myAlg.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] inputBytes = Encoding.Unicode.GetBytes(stringToEncrypt);
            csEncrypt.Write(inputBytes, 0, inputBytes.Length);
            csEncrypt.FlushFinalBlock();
            byte[] encrypted = msEncrypt.ToArray();
            string cipherText = Convert.ToBase64String(encrypted, 0, encrypted.Length);

            return cipherText;
        }

        public void EncryptToFile(string fileFullName, string textToEncrypt)
        {
            try
            {
                using FileStream fileStream = new(fileFullName, FileMode.Create, FileAccess.Write);
                using StreamWriter streamWriter = new(fileStream);
                streamWriter.Write(Encrypt(textToEncrypt));
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message);
            }
        }
    }
}
