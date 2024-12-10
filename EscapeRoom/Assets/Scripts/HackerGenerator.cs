using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HackerGenerator
{
    // Dictionary to store hacker names and their corresponding passwords
    private readonly Dictionary<string, string> hackerProfiles = new Dictionary<string, string>()
    {
        { "TheShadow", "Password" },
        { "AcidBurn", "Elite1337" },
        { "ThePlague", "DaemonVice" },
        { "CerealKiller", "CrunchTheCode" },
        { "PhantomPhreak", "GhostInTheWires" }
    };

    // Selected hacker name and password
    public string name;
    public string password;

    public void PickRandomHackerProfile()
    {
        // Unity's Random is used instead of System.Random
        List<string> names = new List<string>(hackerProfiles.Keys);
        name = names[Random.Range(0, names.Count)];
        password = hackerProfiles[name];

        // Debug output to the Unity Console
        Debug.Log($"Hacker Name: {name}");
        Debug.Log($"Hacker Password: {password}");
    }
}
