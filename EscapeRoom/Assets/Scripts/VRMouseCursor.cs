using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class VRMouseCursor : MonoBehaviour
{
    #region Fields

    public RectTransform canvasRectTransform; // Canvas RectTransform
    public RectTransform cursorImage; // Cursor RectTransform
    public BoxCollider detectionBox; // Detection box for VR controller

    public float sensitivity = 10f; // Movement sensitivity
    public float lerpSpeed = 10f; // Lerp speed
    public float movementThreshold = 0.01f; // Minimum movement to register

    private Vector3 lastPosition; // Last recorded position of the VR controller
    private Vector3 movement; // Movement vector
    private Vector2 targetPosition; // Target canvas position for the cursor
    private GameObject lastHoveredObject; // Last hovered UI object to handle hover exit events
    private bool canMove; // Flag to control movement

    [SerializeField] private InputAction clickAction; // Input action for click

    #endregion

    #region Unity Methods

    private void Start()
    {
        if (canvasRectTransform == null || cursorImage == null || detectionBox == null)
        {
            Debug.LogError("Please assign all required references in the inspector.");
            enabled = false;
            return;
        }

        lastPosition = transform.position;
        targetPosition = cursorImage.anchoredPosition;

        clickAction.Enable();
        clickAction.performed += OnClickPerformed;
    }

    private void Update()
    {
        MoveCursor();
        SimulateHover();
    }

    private void OnDestroy()
    {
        clickAction.performed -= OnClickPerformed;
        clickAction.Disable();
    }

    private void OnDrawGizmos()
    {
        if (detectionBox != null)
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = detectionBox.transform.localToWorldMatrix;
            Gizmos.DrawWireCube(detectionBox.center, detectionBox.size);
        }
    }

    #endregion

    #region Cursor Movement

    private void MoveCursor()
    {
        Vector3 currentPosition = transform.position;
        movement = currentPosition - lastPosition;
        lastPosition = currentPosition;

        if (!canMove || movement.magnitude < movementThreshold)
            return;

        Vector2 canvasMovement = new Vector2(movement.x, movement.z) * sensitivity;
        targetPosition += canvasMovement;

        Rect canvasRect = canvasRectTransform.rect;
        float scaledCursorWidth = cursorImage.sizeDelta.x * cursorImage.localScale.x / 2f;
        float scaledCursorHeight = cursorImage.sizeDelta.y * cursorImage.localScale.y / 2f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, canvasRect.xMin + scaledCursorWidth, canvasRect.xMax - scaledCursorWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, canvasRect.yMin + scaledCursorHeight, canvasRect.yMax - scaledCursorHeight);

        cursorImage.anchoredPosition = Vector2.Lerp(cursorImage.anchoredPosition, targetPosition, lerpSpeed * Time.deltaTime);
    }

    #endregion

    #region Input Handling

    private void OnClickPerformed(InputAction.CallbackContext context)
    {
        HandleClick();
    }

    private void HandleClick()
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorImage.position);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            GameObject clickedObject = raycastResults[0].gameObject;

            ExecuteEvents.Execute(clickedObject, pointerData, ExecuteEvents.pointerClickHandler);
            Debug.Log($"Clicked on: {clickedObject.name}");
        }
        else
        {
            Debug.Log("No clickable UI element found.");
        }
    }

    #endregion

    #region Hover Simulation

    private void SimulateHover()
    {
        Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, cursorImage.position);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (raycastResults.Count > 0)
        {
            GameObject hoveredObject = raycastResults[0].gameObject;

            if (hoveredObject != lastHoveredObject)
            {
                if (lastHoveredObject != null)
                {
                    ExecuteEvents.Execute(lastHoveredObject, pointerData, ExecuteEvents.pointerExitHandler);
                }

                ExecuteEvents.Execute(hoveredObject, pointerData, ExecuteEvents.pointerEnterHandler);
                lastHoveredObject = hoveredObject;

                Debug.Log($"Hovering over: {hoveredObject.name}");
            }
        }
        else if (lastHoveredObject != null)
        {
            ExecuteEvents.Execute(lastHoveredObject, pointerData, ExecuteEvents.pointerExitHandler);
            lastHoveredObject = null;
        }
    }

    #endregion

    #region Collision Handling
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != this)
        {
            canMove = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != this)
        {
            canMove = false;
        }
    }

    #endregion
}