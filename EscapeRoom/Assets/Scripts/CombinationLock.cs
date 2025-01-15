using UnityEngine;
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
    [SerializeField, Tooltip("Door Rigidbody for unlocking the door")] private Rigidbody rb;

    [Header("Input (settings)")]
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference leftSelect;
    [SerializeField, Tooltip("2D Axis input resembling the select action")] InputActionReference rightSelect;
    [SerializeField] float joyThreshold = 0.5f;
    [SerializeField] float maxSelectDelay = 0.5f;

    // invisible variables
    private int digitSideAmount = 10;
    private bool active = false;
    private Vector3 savedLocation;
    private int currDigitIndex = 0;
    private float selectDelay = 0f;


    // storing the saved location
    void Start()
    {
        savedLocation = transform.position;
        indicator.gameObject.SetActive(false);
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
        Debug.Log($"Is Active: {active}     Select Delay: {selectDelay}     Current Digit Index: {currDigitIndex}");
        // reducing select delay
        selectDelay -= selectDelay > 0 ? Time.deltaTime : 0;

        // getting the input vector
        Vector2 input = leftSelect.action.ReadValue<Vector2>() + rightSelect.action.ReadValue<Vector2>(); // players can use both controllers  

        Debug.Log(input);

        // if controller is grabbed and no delay
        if (active && selectDelay <= 0)
        {
            // selecting target digit
            if (input.x > joyThreshold)
            {
                currDigitIndex = currDigitIndex > 0 ? currDigitIndex - 1 : digits.Length - 1;
                selectDelay = maxSelectDelay;
            }
            else if (input.x < -joyThreshold)
            {
                currDigitIndex = currDigitIndex < digits.Length - 1 ? currDigitIndex + 1 : 0;
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
            Transform CurrentDigit = digits[currDigitIndex];
            indicator.localPosition = new Vector3(CurrentDigit.localPosition.x, CurrentDigit.localPosition.y +.65f, CurrentDigit.localPosition.z -1.08059464f);

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

            if (correct)
            {
                unlockAction.Invoke();
                Debug.Log("Code Correct!");
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
        // Calculate the rotation increment
        float rotationStep = 360f / digitSideAmount;

        // Adjust the rotation
        Vector3 newRot = digits[digitIndex].localEulerAngles;
        newRot.x += clockwise ? -rotationStep : rotationStep; // Clockwise rotation in local space

        // Apply the new rotation
        digits[digitIndex].localRotation = Quaternion.Euler(newRot);

        // Update digit value with wrapping
        digitValues[digitIndex] = (digitValues[digitIndex] + (clockwise ? 1 : -1) + digitSideAmount) % digitSideAmount;

        Debug.Log($"Digit {digitIndex} rotated to {newRot.x} degrees. Current Value: {digitValues[digitIndex]}");
    }

    public void Unlock()
    {
        rb.useGravity = true;
        rb.isKinematic = false;

        // Apply torque to rotate on the x-axis
        rb.AddTorque(Vector3.left * 1f, ForceMode.Impulse);
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