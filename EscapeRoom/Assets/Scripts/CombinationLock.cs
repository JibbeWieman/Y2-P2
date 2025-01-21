using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

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
    [SerializeField, Tooltip("Door Rigidbody for unlocking the door")] private Rigidbody rb;
    [SerializeField, Tooltip("Key for inside of the lock.")] private XRGrabInteractable lockedKey;

    [Header("Input (settings)")]
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference leftSelect;
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference rightSelect;
    [SerializeField] float joyThreshold = 0.8f;
    [SerializeField] float maxSelectDelay = 0.5f;

    // invisible variables
    private int digitSideAmount = 10;
    private bool active = false;
    private Vector3 savedLocation;
    private int currDigitIndex = 0;
    private float selectDelay = 0f;
    private bool unlocked = false;


    // storing the saved location
    void Start()
    {
        savedLocation = transform.position;
        indicator.gameObject.SetActive(false);
        lockedKey.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            indicator.gameObject.SetActive(true);
        }

        active = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            indicator.gameObject.SetActive(false);
        }

        active = false;
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
            // Selecting target digit
            if (input.x > joyThreshold)
            {
                currDigitIndex = (currDigitIndex + 1) % digits.Length; // Increment and wrap around
                selectDelay = maxSelectDelay;
            }
            else if (input.x < -joyThreshold)
            {
                currDigitIndex = (currDigitIndex - 1 + digits.Length) % digits.Length; // Decrement and wrap around
                selectDelay = maxSelectDelay;
            }

            // selecting digit rotation
            if (input.y > joyThreshold)
            {
                TurnLock(currDigitIndex, true);
                selectDelay = maxSelectDelay;
            }
            else if (input.y < -joyThreshold)
            {
                TurnLock(currDigitIndex, false);
                selectDelay = maxSelectDelay;
            }

            // displaying currently selected digit
            Transform currentDigit = digits[currDigitIndex];
            indicator.localPosition = currentDigit.localPosition + new Vector3(0, 0.65f, -1.08059464f);


            // confirm combination
            //if (leftConfirm.action.WasPressedThisFrame() || rightConfirm.action.WasPressedThisFrame()) {
            bool correct = true;
            for (int i = 0; i < digitValues.Length; i++)
            {
                if (digitValues[i] != correctCombination[i])
                {
                    correct = false;
                    break; // Exit early if any digit is incorrect
                }
            }

            if (correct && !unlocked)
            {
                unlockAction.Invoke();
            }
            //}
        }
        else if (!active)
        {
            // set position to saved location
            transform.position = savedLocation;
        }
        else if (Mathf.Abs(input.y) < joyThreshold && Mathf.Abs(input.x) < joyThreshold)
        {
            selectDelay = 0;
        }
    }

    public void TurnLock(int digitIndex, bool clockwise)
    {
        float rotationStep = 360f / digitSideAmount;

        // Adjust the rotation directly
        float rotationDirection = clockwise ? -rotationStep : rotationStep;
        digits[digitIndex].Rotate(Vector3.right, rotationDirection, Space.Self);

        // Update digit value with wrapping
        digitValues[digitIndex] = (digitValues[digitIndex] + (clockwise ? 1 : -1) + digitSideAmount) % digitSideAmount;

        Debug.Log($"Digit {digitIndex} rotated. Current Value: {digitValues[digitIndex]}");
    }


    public void Unlock()
    {
        Debug.Log("Code Correct! Unlocking lock...");
        //rb.useGravity = true;
        //rb.isKinematic = false;

        rb.transform.localRotation = Quaternion.Lerp(
        rb.transform.localRotation,
        Quaternion.Euler(rb.transform.localRotation.eulerAngles + new Vector3(-90f, 0f, 0f)),
        0.015f
    );

        // Convert to Euler angles for comparison
        Vector3 currentEulerAngles = rb.transform.localRotation.eulerAngles;

        // Handle 360-degree wrapping issues by normalising the angle
        float xAngle = currentEulerAngles.x;
        if (xAngle > 180f) xAngle -= 360f;

        // Check if the x rotation is within the desired range
        if (xAngle >= -92f && xAngle <= -88f)
        {
            unlocked = true;
            Debug.Log("Lock successfully unlocked!");
        }

        //rb.AddTorque(Vector3.left * 1f, ForceMode.Impulse);

        lockedKey.enabled = true;
    }
}
/* #if UNITY_EDITOR
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
                lockEditor.TurnLock(0, false);
            }
            if (GUILayout.Button("Slot 0 clockwise"))
            {
                lockEditor.TurnLock(0, true);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Slot 1 anti-clockwise"))
            {
                lockEditor.TurnLock(1, false);
            }
            if (GUILayout.Button("Slot 1 clockwise"))
            {
                lockEditor.TurnLock(1, true);
            }
            GUILayout.Space(10);
            if (GUILayout.Button("Slot 2 anti-clockwise"))
            {
                lockEditor.TurnLock(2, false);
            }
            if (GUILayout.Button("Slot 2 clockwise"))
            {
                lockEditor.TurnLock(2, true);
            }

        }
    }
}
#endif */