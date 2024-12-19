using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;

public class HackerGenerator : MonoBehaviour
{
    // Dictionary to store hacker names and their corresponding student names
    private readonly Dictionary<string, string> hackerProfiles = new Dictionary<string, string>()
    {
        { "TheShadow", "Jeff Jefferson" },
        { "CerealKiller", "Anna Smith" },
        { "ThePlague", "Taylor Carter" },
        { "Neo", "Thomas Anderson" },
    };

    // Selected hacker name and student name
    public string hackerName;
    public string studentName;
    public string password;
    public EncryptionMethod encryptionMethod;

    public StickyNote stickyNoteStudentName;
    public StickyNote stickyNoteHackerName;
    public StickyNote stickyNoteEncryptionMethod;

    public enum EncryptionMethod
    {
        Caesar,
        Vigenere,
        AES,
        //RSA,
    }

    public void PickRandomHackerProfile()
    {
        // Select a random hacker profile
        List<string> hackerNames = new List<string>(hackerProfiles.Keys);
        hackerName = hackerNames[UnityEngine.Random.Range(0, hackerNames.Count)];
        studentName = hackerProfiles[hackerName];

        // Generate a password for encryption
        string plainPassword = hackerName; // Use the hacker name as the base password
        encryptionMethod = (EncryptionMethod)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EncryptionMethod)).Length);
        password = EncryptPassword(plainPassword, encryptionMethod);

        // Debug output to the Unity Console
        Debug.Log($"Hacker Name: {hackerName}");
        Debug.Log($"Student Name: {studentName}");
        Debug.Log($"Encrypted Password: {password}");
        Debug.Log($"Encryption Method Used: {encryptionMethod}");

        stickyNoteHackerName.textObj.text = hackerName;
        stickyNoteStudentName.textObj.text = studentName;
        stickyNoteEncryptionMethod.textObj.text = encryptionMethod.ToString();
    }

    private string EncryptPassword(string plainPassword, EncryptionMethod method)
    {
        switch (method)
        {
            case EncryptionMethod.Caesar:
                return CaesarCipherEncrypt(plainPassword, 3); // Shift by 3
            case EncryptionMethod.Vigenere:
                return VigenereCipherEncrypt(plainPassword, "KEY");
            case EncryptionMethod.AES:
                return AESEncrypt(plainPassword, "1234567812345678", "8765432187654321"); // Example 16-byte key and IV
            //case EncryptionMethod.RSA:
            //    return RSAEncrypt(plainPassword, GetRSAPublicKey()); // Use RSA public key
            default:
                return plainPassword;
        }
    }

    private string CaesarCipherEncrypt(string input, int shift)
    {
        char[] buffer = input.ToCharArray();
        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];
            if (char.IsLetter(letter))
            {
                char offset = char.IsUpper(letter) ? 'A' : 'a';
                letter = (char)(((letter + shift - offset) % 26) + offset);
            }
            buffer[i] = letter;
        }
        return new string(buffer);
    }

    private string VigenereCipherEncrypt(string input, string key)
    {
        char[] buffer = input.ToCharArray();
        int keyIndex = 0;
        int keyLength = key.Length;

        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];
            if (char.IsLetter(letter))
            {
                char offset = char.IsUpper(letter) ? 'A' : 'a';
                int shift = (key[keyIndex % keyLength] - offset) % 26;
                letter = (char)(((letter + shift - offset) % 26) + offset);
                keyIndex++;
            }
            buffer[i] = letter;
        }
        return new string(buffer);
    }

    private string AESEncrypt(string input, string key, string iv)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = Encoding.UTF8.GetBytes(iv);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var memoryStream = new System.IO.MemoryStream())
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            {
                using (var writer = new System.IO.StreamWriter(cryptoStream))
                {
                    writer.Write(input);
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    private string RSAEncrypt(string input, string publicKey)
    {
        using (var rsa = new RSACryptoServiceProvider())
        {
            rsa.FromXmlString(publicKey);
            byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(input), false);
            return Convert.ToBase64String(encryptedData);
        }
    }

    private string GetRSAPublicKey()
    {
        // Replace with a real RSA public key
        return @"<RSAKeyValue>
                    <Modulus>woxL2zLsPVjl3XQmNnUnnmLhsKDaV4WxxzJvd+75mTp3zB0Lt4BTkrZYoz/uX8f3X9KUOFAF3r6TTpNZnFnC/x==</Modulus>
                    <Exponent>AQAB</Exponent>
                 </RSAKeyValue>";
    }
}
