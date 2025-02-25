using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    [Tooltip("Possible Student Hacker Profiles")]
    private readonly Dictionary<string, string> studentProfiles = new()
    {
        { "j.jansen@noorderpoort.nl", "Jeff Jansen" },
        { "a.smid@noorderpoort.nl", "Anna Smid" },
        { "r.rijkeboer@noorderpoort.nl", "René Rijkerboer" },
        { "t.bakker@noorderpoort.nl", "Thomas Bakker" },
    };

    [Tooltip("Possible Teacher Log-In Passwords")]
    private readonly List<string> logInPasswords = new()
    {
        { "GeenIdee"},
        { "Wachtwoord" },
        { "School123" },
    };

    // Student hacker information
    public string hackerName, hackerEmail, hackerPassword;
    public string teacherPassword;
    public EncryptionMethod encryptionMethod;

    [SerializeField, Tooltip("Possible Teacher Log-In Sticky Notes - Used for Randomizing the Position")]
    private GameObject[] logInStickyNotes;

    public enum EncryptionMethod
    {
        Caesar,
        Vigenere,
        Atbash,
        RailWayFence,
    }

    #region Randomized Teacher Log-In & Sticky Note Position
    private void Awake()
    {
        RandomizeLogIn();
        RandomizeHacker();
    }

    private void RandomizeLogIn()
    {
        // Disable all sticky notes
        foreach (GameObject stickyNote in logInStickyNotes)
        {
            stickyNote.SetActive(false);
        }

        // Enable a random sticky note and assign a random password
        if (logInStickyNotes.Length > 0)
        {
            int randomStickyNoteIndex = Random.Range(0, logInStickyNotes.Length);
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
    #endregion

    public void RandomizeHacker()
    {
        // Select a random hacker profile
        List<string> hackerEmails = new(studentProfiles.Keys);
        hackerEmail = hackerEmails[UnityEngine.Random.Range(0, hackerEmails.Count)];
        hackerName = studentProfiles[hackerEmail];

        // Generate a password for encryption
        string plainPassword = hackerName;

        encryptionMethod = hackerName switch                // AYO THIS IS A THING???
        {
            "Jeff Jansen" => EncryptionMethod.Caesar,
            "Anna Smid" => EncryptionMethod.RailWayFence,
            "René Rijkeboer" => EncryptionMethod.Vigenere,
            _ => EncryptionMethod.Atbash,
        };

        hackerPassword = EncryptPassword(plainPassword, encryptionMethod);

        // Debug output to the Unity Console
        Debug.Log($"Hacker Name: {hackerName}");
        Debug.Log($"Encrypted Password: {hackerPassword}");
        Debug.Log($"Encryption Method Used: {encryptionMethod}");

        /* 
        encryptionMethod = (EncryptionMethod)UnityEngine.Random.Range(0, Enum.GetValues(typeof(EncryptionMethod)).Length);
        stickyNoteHackerName.textObj.text = hackerName;
        stickyNoteStudentName.textObj.text = studentName;
        stickyNoteEncryptionMethod.textObj.text = encryptionMethod.ToString(); */
    }

    private string EncryptPassword(string plainPassword, EncryptionMethod method)
    {
        return method switch
        {
            EncryptionMethod.Caesar => CaesarCipherEncrypt(plainPassword, 3), // Shift by 3
            EncryptionMethod.Vigenere => VigenereCipherEncrypt(plainPassword, "KEY"), 
            EncryptionMethod.Atbash => AtbashCipherEncrypt(plainPassword),
            EncryptionMethod.RailWayFence => RailwayFenceCipherEncrypt(plainPassword, 3), // Use 3 rails as default
            _ => plainPassword,
        };
    }

    #region Encryption Methods
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
    #endregion
}
