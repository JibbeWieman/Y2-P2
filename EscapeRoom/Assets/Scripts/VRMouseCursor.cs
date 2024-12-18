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
        // Movement calculation (same as before)
        Vector3 currentPosition = transform.position;
        movement = currentPosition - lastPosition;
        lastPosition = currentPosition;

        if (!canMove || rb.velocity.magnitude <= 0.01f || movement.magnitude < movementThreshold)
            return;

        // Convert world movement to canvas space
        Vector2 canvasMovement = new Vector2(movement.x, movement.z) * sensitivity;

        if (canMove)
        {
            // Update target position
            targetPosition = cursorImage.anchoredPosition + canvasMovement;

            // Clamp the position
            Rect canvasRect = canvasRectTransform.rect;
            targetPosition.x = Mathf.Clamp(targetPosition.x, canvasRect.xMin, canvasRect.xMax);
            targetPosition.y = Mathf.Clamp(targetPosition.y, canvasRect.yMin, canvasRect.yMax);
        }

        // Smoothly move cursor
        cursorImage.anchoredPosition = Vector2.Lerp(
            cursorImage.anchoredPosition,
            targetPosition,
            lerpSpeed * Time.deltaTime
        );

        // Simulate hover interactions
        SimulateHover();
    }


    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        HandleClick();
    }

    void HandleClick()
    {
        // Convert cursor position to screen space
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorImage.position);

        Debug.Log($"Click registered at screen position: {screenPosition}");

        // Create PointerEventData for the EventSystem
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        // Perform a raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        foreach (var result in raycastResults)
        {
            Debug.Log($"{result.gameObject.name}");
        }
            EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            Debug.Log($"Raycast hit: {result.gameObject.name}");

            // Check if the object is interactive (Button, Toggle, etc.)
            if (result.gameObject.TryGetComponent<Button>(out var button))
            {
                Debug.Log($"Button clicked: {button.name}");
                button.onClick.Invoke();
                return; // Stop further processing after first valid click
            }
        }

        Debug.Log("No interactive UI element clicked.");
    }


    void SimulateHover()
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorImage.position);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        // Perform a raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            foreach (var result in raycastResults)
            {
                GameObject hoveredObject = result.gameObject;

                // Handle hover states if the object is a button
                if (hoveredObject.TryGetComponent<Button>(out var button))
                {
                    ExecuteEvents.Execute(hoveredObject, pointerData, ExecuteEvents.pointerEnterHandler);
                    Debug.Log($"Hovering over: {hoveredObject.name}");
                }
            }
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
