using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class VRMouseCursor : MonoBehaviour
{
    public RectTransform canvasRectTransform; // The RectTransform of the canvas
    public RectTransform cursorImage; // The RectTransform of the cursor image
    public BoxCollider detectionBox; // The BoxCollider for detecting objects

    public float sensitivity = 10f; // Sensitivity factor to control cursor movement
    public float lerpSpeed = 10f; // Speed of lerping (higher is faster)
    public float movementThreshold = 0.01f; // Ignore very small movements

    private Vector3 lastPosition;
    private Vector3 movement;
    private Rigidbody rb;
    private Vector2 targetPosition; // Stores the target position for lerping
    private bool canMove;

    // Input Action for VR Controller Click
    [SerializeField] private InputAction clickAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (canvasRectTransform == null || cursorImage == null || detectionBox == null)
        {
            Debug.LogError("Please assign all required references in the inspector.");
            enabled = false;
        }

        lastPosition = transform.position;
        targetPosition = cursorImage.anchoredPosition; // Initialise target position

        // Enable and bind the click action
        clickAction.Enable();
        clickAction.performed += OnClickPerformed;
    }

    void Update()
    {
        // Calculate movement
        Vector3 currentPosition = transform.position;
        movement = currentPosition - lastPosition;
        lastPosition = currentPosition;

        // Ignore tiny movements or if velocity is negligible
        if (!canMove || rb.velocity.magnitude <= 0.01f || movement.magnitude < movementThreshold)
            return;

        // Convert world movement to canvas space
        Vector2 canvasMovement = new Vector2(movement.x, movement.z) * sensitivity;

        // Check for objects underneath the BoxCollider
        if (canMove)
        {
            // Calculate the new target position
            targetPosition = cursorImage.anchoredPosition + canvasMovement;

            // Clamp the target position within the canvas bounds
            Rect canvasRect = canvasRectTransform.rect;
            targetPosition.x = Mathf.Clamp(targetPosition.x, canvasRect.xMin, canvasRect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, canvasRect.yMin, canvasRect.yMax);
        }

        // Smoothly lerp the cursor to the target position
        cursorImage.anchoredPosition = Vector2.Lerp(
            cursorImage.anchoredPosition,
            targetPosition,
            lerpSpeed * Time.deltaTime
        );
    }

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        HandleClick();
    }

    void HandleClick()
    {
        // Get the position of the cursor in canvas space
        Vector2 clickPosition = cursorImage.anchoredPosition;

        // Convert to screen position for UI interactions
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorImage.position);

        Debug.Log($"Click registered at screen position: {screenPosition}");

        // Create PointerEventData with the converted screen position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        // Perform a raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            // Get the topmost UI element hit by the raycast
            RaycastResult result = raycastResults[0];
            GameObject clickedObject = result.gameObject;

            Debug.Log($"Clicked on: {clickedObject.name}");

            // If the clicked object has a Button component, invoke its click event
            Button button = clickedObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
                Debug.Log("Button click event invoked.");
            }
            else
            {
                Debug.Log("Clicked object is not a button.");
            }
        }
        else
        {
            Debug.Log("No UI element clicked.");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        if (other.gameObject != this)
        {
            canMove = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        if (other.gameObject != this)
        {
            canMove = false;
        }
    }


    void OnDestroy()
    {
        clickAction.performed -= OnClickPerformed;
        clickAction.Disable();
    }

    void OnDrawGizmos()
    {
        if (detectionBox != null)
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = detectionBox.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(detectionBox.center, detectionBox.size);
        }
    }

}
