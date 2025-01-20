using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CheckKey : MonoBehaviour
{
    [SerializeField]
    private BoxCollider checkKeyArea;

    [SerializeField]
    private SceneTypeObject requiredObject;

    private GameObject requiredKey;

    [SerializeField]
    private XRGrabInteractable XRGrabInteractable;

    [SerializeField]
    private List<GameObject> LockedObjects;

    private GameObject insertedObject;

    [SerializeField]
    private GameObject LosePanel, text;

    private void Start()
    {
        checkKeyArea = GetComponent<BoxCollider>();
        
        if (XRGrabInteractable != null)
            XRGrabInteractable.enabled = false;
        LockObjects(true);

        requiredKey = requiredObject.Objects[0];
    }

    public void CheckIfKey()
    {
        if (insertedObject == requiredKey)
        {
            UnlockLock();
        }
    }

    public void UnlockLock()
    {
        //something
        Debug.Log("Correct key inserted");
        if (XRGrabInteractable != null)
            XRGrabInteractable.enabled = true;

        if (insertedObject != null && insertedObject.CompareTag("USB Killer"))
        {
            LosePanel.SetActive(true);
            text.SetActive(true);
        }

        LockObjects(false);
    }

    private void LockObjects(bool locked)
    {
        foreach (GameObject lockedObject in LockedObjects)
        {
            lockedObject.GetComponent<XRGrabInteractable>().enabled = !locked;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        insertedObject = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        insertedObject = null;
    }
}
