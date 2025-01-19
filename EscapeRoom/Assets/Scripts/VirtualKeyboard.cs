// Iterations: Shift using pointerUp, Using PanelGroup for cleaner Inspector, Cleaning up code using helper methods

using System;
using System.Collections;
using TMPro;
using Unity.FPS.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[System.Serializable]
public class PanelGroup
{
    public GameObject LogInPanel;
    public GameObject MainPanel;
    public GameObject SearchPanel;
    public GameObject StudentLogInPanel;
    public GameObject FileExplorerPanel;
    public GameObject CloudFilesPanel;
    public GameObject StudentAccPanel;
    public GameObject Succes;
    public GameObject Fail;

    public GameObject[] AllPanels => new GameObject[]
    {
        LogInPanel,
        MainPanel,
        SearchPanel,
        StudentLogInPanel,
        FileExplorerPanel,
        CloudFilesPanel,
        StudentAccPanel,
        Succes,
        Fail,
    };
}


public class VirtualKeyboard : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private int characterLimit = 20;

    [Header("Laptop UI Panels")]
    [SerializeField] private PanelGroup panels;

    [Header("Audio")]
    [SerializeField] private AudioClip successSFX;
    [SerializeField] private AudioClip failureSFX;

    [Header("Interactors")]
    [SerializeField]
    private NearFarInteractor leftRayInteractor;
    [SerializeField]
    private NearFarInteractor rightRayInteractor;

    private AudioSource audioSource;
    private Randomizer randomizer;
    private HelpLine helpLine;

    private TMP_InputField activeInputField;
    private Coroutine fadeCoroutine;

    private bool loggedIn = false;
    private bool nameGuessed = false;
    private bool isCapsLockOn = false;
    private string teacherPassword;

    void Start()
    {
        // Set listener to click event for setting the Active Input Field
        EventManager.AddListener<ClickEvent>(SetActiveInputField);

        audioSource = FindAnyObjectByType<AudioSource>();
        randomizer = FindAnyObjectByType<Randomizer>();
        helpLine = FindAnyObjectByType<HelpLine>();

        // Ensure panel visibility on start & get input fields
        foreach (GameObject panel in panels.AllPanels)
        {
            panel?.SetActive(false);
            TMP_InputField[] _ = GetComponentsInChildren<TMP_InputField>();

            foreach (TMP_InputField field in _)
            {
                SetInputField(field);
            }
        }
        panels.LogInPanel?.SetActive(true);
        activeInputField = panels.LogInPanel.GetComponentInChildren<TMP_InputField>();

        // Get the randomized teacher log-in password
        teacherPassword = randomizer.teacherPassword;
    }

    private void ValidateAnswer(string inputText)
    {
        bool isCorrect = false;

        if (!loggedIn)
        {
            if (inputText == teacherPassword)
            {
                loggedIn = true;
                helpLine.SetParameter(HelpLine.Parameters.LoggedIn, true);
                panels.MainPanel.SetActive(true);
                panels.LogInPanel.SetActive(false);
                isCorrect = true;
                Console.WriteLine("Teacher logged in. Opening Student Login Panel.");
            }
            else
            {
                Console.WriteLine("Incorrect teacher password. Clearing input field.");
            }
        }
        else
        {
            if (!nameGuessed && inputText == randomizer.hackerName)
            {
                nameGuessed = true;
                isCorrect = true;
                Console.WriteLine("Name guessed correctly! Now guess the password.");
            }
            else if (nameGuessed && inputText == randomizer.hackerPassword)
            {
                isCorrect = true;
                Console.WriteLine("Password guessed correctly! Access granted.");
            }
            else
            {
                Console.WriteLine("Input is incorrect. Try again.");
            }
        }

        activeInputField.text = null;
        audioSource.PlayOneShot(isCorrect ? successSFX : failureSFX);

        // Handle image display and fading
        ShowFeedbackImage(isCorrect);
    }

    #region PASSWORD VISUAL FEEDBACK
    private void ShowFeedbackImage(bool isPositive)
    {
        GameObject targetImage = isPositive ? panels.Succes : panels.Fail;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        targetImage.SetActive(true);
        fadeCoroutine = StartCoroutine(FadeAndHideImage(targetImage, 1f));
    }

    private IEnumerator FadeAndHideImage(GameObject image, float duration)
    {
        CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>() ?? image.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        image.SetActive(false);
    }
    #endregion

    #region KEYBOARD KEY METHODS
    // This method is called when a key is pressed
    public void OnKeyPress(string key)
    {
        if (activeInputField != null && activeInputField.text.Length < characterLimit)
        {
            // Adjust key based on Caps Lock state
            string adjustedKey = isCapsLockOn ? key.ToUpper() : key.ToLower();
            activeInputField.text += adjustedKey;
        }

        Debug.Log($"Key Pressed: {key} (Caps Lock: {isCapsLockOn})"); // Logs the key press for debugging
    }

    // Method for toggling Caps Lock
    public void ToggleCapsLock()
    {
        isCapsLockOn = !isCapsLockOn;
        Debug.Log($"Caps Lock is now {(isCapsLockOn ? "ON" : "OFF")}");
    }

    // Method for handling the spacebar
    public void OnSpacePress()
    {
        if (activeInputField != null)
        {
            activeInputField.text += " "; // Add a space
        }

        Debug.Log("Space Pressed");
    }

    // Method for handling the backspace key
    public void OnBackspacePress()
    {
        if (activeInputField != null && activeInputField.text.Length > 0)
        {
            // Remove the last character
            activeInputField.text = activeInputField.text.Substring(0, activeInputField.text.Length - 1);
        }

        Debug.Log("Backspace Pressed");
    }

    // Method for handling the enter/confirm key
    public void OnEnterPress()
    {
        if (activeInputField != null)
        {
            Debug.Log($"Input Confirmed: {activeInputField.text}");
            ValidateAnswer(activeInputField.text);
        }

        Debug.Log("Enter Pressed");
    }

    // Method for handling Shift key press
    /*public void OnShiftPress(bool isPressed)
    {
        if (isPressed)
        {
            // Shift pressed: Temporarily toggle Caps Lock
            isShiftHeld = true;
            isCapsLockOn = !isCapsLockOn;
        }
        else
        {
            // Shift released: Revert Caps Lock state
            isShiftHeld = false;
            isCapsLockOn = !isCapsLockOn;
        }

        Debug.Log($"Shift {(isPressed ? "Pressed" : "Released")}, Caps Lock: {isCapsLockOn}");
    }*/
    #endregion

    #region HELPER METHODS
    /// <summary>
    /// Check which Input Field is active. Neccessary for the keyboard key methods.
    /// </summary>
    /// 
    private void SetActiveInputField(ClickEvent evt)
    {
        Debug.Log("Setting Active Input Field");

        // Process left and right ray interactors
        ProcessRayInteractor(leftRayInteractor);
        ProcessRayInteractor(rightRayInteractor);
    }

    private void ProcessRayInteractor(NearFarInteractor rayInteractor)
    {
        if (rayInteractor.TryGetCurrentUIRaycastResult(out RaycastResult uiHit))
        {
            if (uiHit.gameObject != null)
            {
                // Try to get the TMP_InputField from the parent of the hit game object
                TMP_InputField input = uiHit.gameObject.GetComponentInParent<TMP_InputField>();

                if (input != null)
                {
                    Debug.Log(input);
                    activeInputField = input;
                }
                else
                {
                    Debug.LogWarning("TMP_InputField not found on the parent.");
                }

                Debug.Log(uiHit.gameObject);
            }
        }
    }

    private void SetInputField(TMP_InputField inputField)
    {
        inputField.characterLimit = characterLimit;
        inputField.onSubmit.AddListener(ValidateAnswer);
    }

    public void SwitchPanels(GameObject from, GameObject to)
    {
        from.SetActive(false);
        to.SetActive(true);
    }
    #endregion

    private void OnDestroy()
    {
        EventManager.RemoveListener<ClickEvent>(SetActiveInputField);
    }
}