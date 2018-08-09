namespace HomeManagement.Contracts
{
    public interface ICryptography
    {
        string Encrypt(string value);

        string Decrypt(string value);

        byte[] GetRawEncrypted(string value);

        string GetRawDecrypted(byte[] encrypted);
    }
}
