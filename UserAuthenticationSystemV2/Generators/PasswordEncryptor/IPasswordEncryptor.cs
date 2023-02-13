namespace UserAuthenticationSystemV2.Generators.PasswordEncryptor
{
    public interface IPasswordEncryptor
    {
        string Encrypt(string plainText);

        string Decrypt(string cipherText);
    }
}