using System;
using TMPro;
using Unity.FPS.Game;
using UnityEngine;

public class VirtualKeyboard : MonoBehaviour
{
    [SerializeField]
    private int characterLimit = 20;

    [SerializeField]
    private GameObject LogInPanel, MainScreenPanel, SearchPanel, StudentLogInPanel, FileExplorerPanel, CloudFilesPanel, StudentAccPanel;

    [Space(5)]

    [SerializeField]
    private HelpLine helpLine;

    private Randomizer hackerID;

    private bool loggedIn = false;
    private bool nameGuessed = false; // Bool to track the match status.
    //private bool passwordGuessed = false; // Bool to track the match status.
    private bool isCapsLockOn = false; // Tracks the state of Caps Lock.
    private string teacherPassword;
    //private bool isShiftHeld = false; // Tracks if Shift is currently held down.

    private TMP_InputField inputField;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip successSFX, failureSFX;

    void Start()
    {
        StudentLogInPanel.SetActive(false);
        hackerID = FindAnyObjectByType<Randomizer>();
        inputField = LogInPanel.GetComponentInChildren<TMP_InputField>();

        teacherPassword = hackerID.teacherPassword;

        inputField.characterLimit = characterLimit;
        inputField.onSubmit.AddListener(ValidateAnswer);

        hackerID.PickRandomHackerProfile();

        audioSource = FindAnyObjectByType<AudioSource>();
    }

    private void ValidateAnswer(string inputText)
    {
        if (!loggedIn)
        {
            // Check teacher password
            if (inputText == teacherPassword)
            {
                helpLine.SetParameter(HelpLine.Parameters.LoggedIn, true);
                loggedIn = true;
                inputField.text = null;
                Console.WriteLine("Teacher logged in. Opening Student Login Panel.");
                audioSource.PlayOneShot(successSFX);
                MainScreenPanel.SetActive(true);
                LogInPanel.SetActive(false);
                inputField = StudentLogInPanel.GetComponentInChildren<TMP_InputField>();
            }
            else
            {
                Console.WriteLine("Incorrect teacher password. Clearing input field.");
                audioSource.PlayOneShot(failureSFX);
                inputField.text = null;
            }
        }
        else
        {
            // Proceed with existing logic if logged in
            switch (nameGuessed)
            {
                case false:
                    if (inputText == hackerID.studentName)
                    {
                        nameGuessed = true;
                        inputField.text = null;
                        Console.WriteLine("Name guessed correctly! Now guess the password.");
                        audioSource.PlayOneShot(successSFX);
                    }
                    else
                    {
                        Console.WriteLine("Name is incorrect. Try again.");
                        audioSource.PlayOneShot(failureSFX);
                    }
                    break;

                case true:
                    if (inputText == hackerID.hackerPassword)
                    {
                        //passwordGuessed = true;
                        inputField.text = null;
                        Console.WriteLine("Password guessed correctly! Access granted.");
                        audioSource.PlayOneShot(successSFX);
                    }
                    else
                    {
                        Console.WriteLine("Password is incorrect. Try again.");
                        audioSource.PlayOneShot(failureSFX);
                    }
                    break;
            }
        }
    }

    public void SwitchPanels(GameObject from, GameObject to)
    {
        from.SetActive(false);
        to.SetActive(true);
    }

    #region BUTTON METHODS
    // This method is called when a key is pressed
    public void OnKeyPress(string key)
    {
        if (inputField != null && inputField.text.Length < characterLimit)
        {
            // Adjust key based on Caps Lock state
            string adjustedKey = isCapsLockOn ? key.ToUpper() : key.ToLower();
            inputField.text += adjustedKey;
        }

        Debug.Log($"Key Pressed: {key} (Caps Lock: {isCapsLockOn})"); // Logs the key press for debugging
    }

    // Method for toggling Caps Lock
    public void ToggleCapsLock()
    {
        isCapsLockOn = !isCapsLockOn;
        Debug.Log($"Caps Lock is now {(isCapsLockOn ? "ON" : "OFF")}");
    }

    // Method for handling Shift key press
    //public void OnShiftPress(bool isPressed)
    //{
    //    if (isPressed)
    //    {
    //        // Shift pressed: Temporarily toggle Caps Lock
    //        isShiftHeld = true;
    //        isCapsLockOn = !isCapsLockOn;
    //    }
    //    else
    //    {
    //        // Shift released: Revert Caps Lock state
    //        isShiftHeld = false;
    //        isCapsLockOn = !isCapsLockOn;
    //    }

    //    Debug.Log($"Shift {(isPressed ? "Pressed" : "Released")}, Caps Lock: {isCapsLockOn}");
    //}

    // Method for handling the spacebar
    public void OnSpacePress()
    {
        if (inputField != null)
        {
            inputField.text += " "; // Add a space
        }

        Debug.Log("Space Pressed");
    }

    // Method for handling the backspace key
    public void OnBackspacePress()
    {
        if (inputField != null && inputField.text.Length > 0)
        {
            // Remove the last character
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }

        Debug.Log("Backspace Pressed");
    }

    // Method for handling the enter/confirm key
    public void OnEnterPress()
    {
        if (inputField != null)
        {
            Debug.Log($"Input Confirmed: {inputField.text}");
            ValidateAnswer(inputField.text);
        }

        Debug.Log("Enter Pressed");
    }
    #endregion
}