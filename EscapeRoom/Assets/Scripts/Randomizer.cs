using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    // Dictionary to store hacker names and their corresponding student names
    private readonly Dictionary<string, string> hackerProfiles = new()
    {
        { "TheShadow", "Jeff Jansen" },
        { "CerealKiller", "Anna Smid" },
        { "ThePlague", "René Carter" },
        { "Neo", "Thomas Bakker" },
    };

    // Selected hacker name and student name
    public string hackerName;
    public string studentName;
    public string hackerPassword;
    public string teacherPassword;
    public EncryptionMethod encryptionMethod;

    public StickyNote stickyNoteStudentName;
    public StickyNote stickyNoteHackerName;
    public StickyNote stickyNoteEncryptionMethod;

    [SerializeField]
    private GameObject[] logInStickyNotes;
    private readonly List<string> logInPasswords = new()
    {
        { "GeenIdee"},
        { "Wachtwoord" },
        { "School123" },
    };

    public enum EncryptionMethod
    {
        Caesar,
        Vigenere,
        Atbash,
        RailWayFence,
    }

    private void Awake()
    {
        RandomizedLogIn();
    }

    private void RandomizedLogIn()
    {
        // Disable all sticky notes
        foreach (GameObject stickyNote in logInStickyNotes)
        {
            stickyNote.SetActive(false);
        }

        // Enable a random sticky note and assign a random password
        if (logInStickyNotes.Length > 0)
        {
            int randomStickyNoteIndex = UnityEngine.Random.Range(0, logInStickyNotes.Length);
            StickyNote selectedStickyNote = logInStickyNotes[randomStickyNoteIndex].GetComponent<StickyNote>();
            selectedStickyNote.gameObject.SetActive(true);

            // Pick a random login password from the list
            if (logInPasswords.Count > 0)
            {
                teacherPassword = logInPasswords[UnityEngine.Random.Range(0, logInPasswords.Count)];
                selectedStickyNote.textObj.text = "Log In: " + teacherPassword;
            }
        }
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
        hackerPassword = EncryptPassword(plainPassword, encryptionMethod);

        // Debug output to the Unity Console
        Debug.Log($"Hacker Name: {hackerName}");
        Debug.Log($"Student Name: {studentName}");
        Debug.Log($"Encrypted Password: {hackerPassword}");
        Debug.Log($"Encryption Method Used: {encryptionMethod}");

        //stickyNoteHackerName.textObj.text = hackerName;
        //stickyNoteStudentName.textObj.text = studentName;
        //stickyNoteEncryptionMethod.textObj.text = encryptionMethod.ToString();
    }

    private string EncryptPassword(string plainPassword, EncryptionMethod method)
    {
        switch (method)
        {
            case EncryptionMethod.Caesar:
                return CaesarCipherEncrypt(plainPassword, 3); // Shift by 3
            case EncryptionMethod.Vigenere:
                return VigenereCipherEncrypt(plainPassword, "KEY");
            case EncryptionMethod.Atbash:
                return AtbashCipherEncrypt(plainPassword);
            case EncryptionMethod.RailWayFence:
                return RailwayFenceCipherEncrypt(plainPassword, 3); // Use 3 rails as default
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

    private string AtbashCipherEncrypt(string input)
    {
        char[] buffer = input.ToCharArray();

        for (int i = 0; i < buffer.Length; i++)
        {
            char letter = buffer[i];

            if (char.IsLetter(letter))
            {
                char offset = char.IsUpper(letter) ? 'A' : 'a';
                letter = (char)(offset + ('Z' - char.ToUpper(letter)));
            }
            buffer[i] = letter;
        }

        return new string(buffer);
    }

    private string RailwayFenceCipherEncrypt(string input, int numRails)
    {
        if (numRails <= 1) return input;

        List<StringBuilder> rails = new();
        for (int i = 0; i < numRails; i++)
            rails.Add(new StringBuilder());

        int currentRail = 0;
        int direction = 1; // 1 for down, -1 for up

        foreach (char c in input)
        {
            rails[currentRail].Append(c);
            currentRail += direction;

            if (currentRail == 0 || currentRail == numRails - 1)
                direction *= -1;
        }

        StringBuilder encrypted = new();
        foreach (var rail in rails)
        {
            encrypted.Append(rail);
        }

        return encrypted.ToString();
    }
}
