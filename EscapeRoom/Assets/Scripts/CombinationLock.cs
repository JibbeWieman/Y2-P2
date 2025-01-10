using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    [Header("Components")]
    [SerializeField, Tooltip("Drag in the object that indicates the current digit")] Transform indicator;
    [SerializeField, Tooltip("Drag in Transforms of digits")] Transform[] digits = new Transform[3];

    [Header("Settings")]
    [SerializeField, Tooltip("Fill in Starting Values")] int[] digitValues = new int[3];
    [SerializeField, Tooltip("Fill in the Correct Combination")] int[] correctCombination = new int[3];

    [Header("Unlock Action")]
    [SerializeField, Tooltip("Action that happens when lock opens")] UnityEvent unlockAction;

    [Header("Input (settings)")]
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference leftSelect;
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference rightSelect;
    [SerializeField, Tooltip("Button input resembling the confirm action")] InputActionReference leftConfirm;
    [SerializeField, Tooltip("Button input resembling the confirm action")] InputActionReference rightConfirm;
    [SerializeField] float joyThreshold = 0.5f;
    [SerializeField] float maxSelectDelay = 0.5f;

    // invisible variables
    int digitSideAmount = 10;
    bool active = false;
    Vector3 savedLocation;
    int currDigitIndex = 0;
    float selectDelay = 0f;


    // storing the saved location
    void Start()
    {
        savedLocation = transform.position;
        indicator.gameObject.SetActive(false);
    }

    // getting the input
    void Update()
    {
        // reducing select delay
        selectDelay -= selectDelay > 0 ? Time.deltaTime : 0;

        // getting the input vector
        Vector2 input = leftSelect.action.ReadValue<Vector2>() + rightSelect.action.ReadValue<Vector2>(); // players can use both controllers  

        // if controller is grabbed and no delay
        if (active && selectDelay <= 0)
        {
            // enabling the indicator
            indicator.gameObject.SetActive(true);

            // selecting target digit
            if (input.y > joyThreshold)
            {
                currDigitIndex = currDigitIndex > 0 ? currDigitIndex - 1 : digits.Length - 1;
                selectDelay = maxSelectDelay;
            }
            else if (input.y < -joyThreshold)
            {
                currDigitIndex = currDigitIndex < digits.Length - 1 ? currDigitIndex + 1 : 0;
                selectDelay = maxSelectDelay;
            }

            // selecting digit rotation
            if (input.x > joyThreshold)
            {
                turnLock(currDigitIndex, true);
                selectDelay = maxSelectDelay;
            }
            else if (input.x < -joyThreshold)
            {
                turnLock(currDigitIndex, false);
                selectDelay = maxSelectDelay;
            }

            // displaying currently selected digit
            indicator.localPosition = new Vector3(indicator.localPosition.x, digits[currDigitIndex].localPosition.y, indicator.localPosition.z);

            // confirm combination
            //if (leftConfirm.action.WasPressedThisFrame() || rightConfirm.action.WasPressedThisFrame()) {
            bool correct = true;

            // looping all digits to check if any are false
            for (int i = 0; i < digitValues.Length; i++)
            {
                if (digitValues[i] != correctCombination[i])
                    correct = false;
            }

            // if all are correct unlock
            if (correct)
                unlockAction.Invoke();
            //}
        }
        else if (!active)
        {
            // set position to saved location
            transform.position = savedLocation;

            // disabling the indicator
            indicator.gameObject.SetActive(true);
        }
        else
        {
            // resetting delay if no button pressed
            if (-joyThreshold < input.y && input.y < joyThreshold && -joyThreshold < input.x && input.x < joyThreshold)
                selectDelay = 0;
        }
    }

    public void GrabLock(bool grabbed)
    {
        active = grabbed;
    }

    public void turnLock(int digitIndex, bool clockwise)
    {
        // turning the digit around
        Vector3 newRot = digits[digitIndex].localEulerAngles;
        newRot.y += 360 / digitSideAmount * (clockwise ? 1 : -1);
        digits[digitIndex].localRotation = Quaternion.Euler(newRot);

        // updating the value
        digitValues[digitIndex] += clockwise ? 1 : -1;
        if (digitValues[digitIndex] < 0 || digitValues[digitIndex] > 9)
        { // looping back
            digitValues[digitIndex] = digitValues[digitIndex] < 0 ? 9 : 0;
        }
    }
}

// Buttons in the inspector
[CustomEditor(typeof(CombinationLock))]
public class CombLockEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (Application.isPlaying)
        {
            CombinationLock lockEditor = (CombinationLock)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Slot 0 anti-clockwise"))
            {
                lockEditor.turnLock(0, false);
            }
            if (GUILayout.Button("Slot 0 clockwise"))
            {
                lockEditor.turnLock(0, true);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Slot 1 anti-clockwise"))
            {
                lockEditor.turnLock(1, false);
            }
            if (GUILayout.Button("Slot 1 clockwise"))
            {
                lockEditor.turnLock(1, true);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Slot 2 anti-clockwise"))
            {
                lockEditor.turnLock(2, false);
            }
            if (GUILayout.Button("Slot 2 clockwise"))
            {
                lockEditor.turnLock(2, true);
            }

        }
    }
}