using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CheckKey : MonoBehaviour
{
    [SerializeField]
    private BoxCollider checkKeyArea;

    [SerializeField]
    private GameObject requiredKey;

    [SerializeField]
    private XRGrabInteractable XRGrabInteractable;

    private GameObject insertedObject;

    private void Start()
    {
        checkKeyArea = GetComponent<BoxCollider>();
        
        if (XRGrabInteractable != null)
            XRGrabInteractable.enabled = false;
    }

    public void CheckIfKey()
    {
        if (insertedObject == requiredKey)
        {
            UnlockLock();
        }
    }

    private void UnlockLock()
    {
        //something
        Debug.Log("Correct key inserted");
        if (XRGrabInteractable != null)
            XRGrabInteractable.enabled = true;
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
