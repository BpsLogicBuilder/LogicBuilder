namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IEncryption
    {
        string Decrypt(string stringToDecrypt);
        string Encrypt(string stringToEncrypt);
        string DecryptFromFile(string fileFullName);
        void EncryptToFile(string fileFullName, string textToEncrypt);
    }
}
