using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StudentFile : MonoBehaviour
{
    [Header("Assign the files GameObjects")]
    public List<GameObject> files; // List of GameObjects containing TMP components

    [Header("Hacker Generator Reference")]
    public Randomizer hackerGenerator;

    private Dictionary<string, string> studentProfiles = new Dictionary<string, string>()
    {
        { "John Doe", "Student File: Jeff Jefferson\nName: Jeff Jefferson\nAge: 19\nGender/Pronouns: Male - he/him\nClass: IT-03\nSchool Email: j.jefferson@noorderpoort.nl\nIQ: 120\nFavourite Encryption Type: Caesar Cipher\nHobbies: Coding, playing chess, and hiking\nNote: Likes to encrypt everything, even his hiking routes." },
        { "Jane Smith", "Student File: Anna Smith\nName: Anna Smith\nAge: 18\nGender/Pronouns: Female - she/her\nClass: IT-01\nSchool Email: a.smith@noorderpoort.nl\nIQ: 110\nFavourite Encryption Type: AES\nHobbies: Reading, painting, and trying out encryption puzzles\nNote: Has a habit of turning her favourite books into encrypted messages for fun." },
        { "Sam Brown", "Student File: Taylor Carter\nName: Taylor Carter\nAge: 22\nGender/Pronouns: Non-binary - they/them\nClass: IT-04\nSchool Email: t.carter@noorderpoort.nl\nIQ: 130\nFavourite Encryption Type: Vigenère Cipher\nHobbies: Programming, gaming, and cryptography\nNote: Encrypts their name into an anagram whenever possible." },
        { "Alex Johnson", "Student File: Thomas Anderson\nName: Thomas Anderson\nAge: 20\nGender/Pronouns: Male - he/him\nClass: IT-02\nSchool Email: t.anderson@noorderpoort.nl\nIQ: 125\nFavourite Encryption Type: RSA\nHobbies: Basketball, solving ciphers, and watching documentaries\nNote: Often challenges classmates to decipher his encrypted notes." }
    };

    void Start()
    {
        UpdateStudentFiles();
    }

    public void UpdateStudentFiles()
    {
        // Generate a random hacker profile
        hackerGenerator.PickRandomHackerProfile();

        // Get the student profile based on the selected hacker
        string studentName = hackerGenerator.studentName;
        if (!studentProfiles.ContainsKey(studentName))
        {
            Debug.LogError($"Student profile not found for {studentName}");
            return;
        }

        string updatedProfile = studentProfiles[studentName];

        // Update one random file GameObject with the student info
        foreach (var file in files)
        {
            TextMeshProUGUI textMesh = file.GetComponentInChildren<TextMeshProUGUI>();
            if (textMesh != null)
            {
                textMesh.text = updatedProfile;
                Debug.Log($"Updated file: {file.name} with student info: {studentName}");
                break; // Update only one file
            }
        }
    }
}
