using GrowthWare.Framework.Enumerations;

namespace GrowthWare.Framework.Tests;

[TestFixture]
public class CryptoUtilityTests
{
    private const string m_TestString = "Hello, World!";
    private string m_EncryptedValue = string.Empty;

    [SetUp]
    public void Setup()
    {
        // This method runs before each test
    }

    [Test]
    public void Test_Aes_Encryption_And_Decryption()
    {
        // Act
        bool isEncrypted = CryptoUtility.TryEncrypt(m_TestString, out m_EncryptedValue, EncryptionType.Aes);
        Assert.That(isEncrypted, Is.True, "Encryption failed.");

        // Act
        bool isDecrypted = CryptoUtility.TryDecrypt(m_EncryptedValue, out string decryptedValue, EncryptionType.Aes);
        Assert.That(isDecrypted, Is.True, "Decryption failed.");
        Assert.That(decryptedValue, Is.EqualTo(m_TestString), "Decrypted value does not match the original.");
    }

    [Test]
    public void Test_Des_Encryption_And_Decryption()
    {
        // Act
        bool isEncrypted = CryptoUtility.TryEncrypt(m_TestString, out m_EncryptedValue, EncryptionType.Des);
        Assert.That(isEncrypted, Is.True, "Encryption failed.");

        // Act
        bool isDecrypted = CryptoUtility.TryDecrypt(m_EncryptedValue, out string decryptedValue, EncryptionType.Des);
        Assert.That(isDecrypted, Is.True, "Decryption failed.");
        Assert.That(decryptedValue, Is.EqualTo(m_TestString), "Decrypted value does not match the original.");
    }

    [Test]
    public void Test_TripleDes_Encryption_And_Decryption()
    {
        // Act
        bool isEncrypted = CryptoUtility.TryEncrypt(m_TestString, out m_EncryptedValue, EncryptionType.TripleDes);
        Assert.That(isEncrypted, Is.True, "Encryption failed.");

        // Act
        bool isDecrypted = CryptoUtility.TryDecrypt(m_EncryptedValue, out string decryptedValue, EncryptionType.TripleDes);
        Assert.That(isDecrypted, Is.True, "Decryption failed.");
        Assert.That(decryptedValue, Is.EqualTo(m_TestString), "Decrypted value does not match the original.");
    }

    [Test]
    public void Test_Aes_Encryption_Invalid_Data()
    {
        // Arrange
        // this was the encrypted value from Test_Aes_Encryption_And_Decryption with the last character removed
        string mTestString = "SI4gm8D1XRAE2f+Q80atY72+NKOAU2FcD3oka2rU4z8";
        // Act
        bool isEncrypted = CryptoUtility.TryDecrypt(mTestString, out string result, EncryptionType.Aes);
        Assert.That(isEncrypted, Is.True, "Encryption should not fail if the data is not encrypted properly.");
        Assert.That(result, Is.EqualTo(mTestString), "Decrypted value does not match the original.");
    }
}