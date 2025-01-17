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
    private GameObject requiredKey;

    [SerializeField]
    private XRGrabInteractable XRGrabInteractable;

    [SerializeField]
    private GameObject[] papers;

    private GameObject insertedObject;

    private void Start()
    {
        checkKeyArea = GetComponent<BoxCollider>();
        
        if (XRGrabInteractable != null)
            XRGrabInteractable.enabled = false;
        setPaperLocked(true);
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
#if UNITY_EDITOR
            Debug.Log("Detected USB Killer in Unity Editor. Stopping play mode.");
            EditorApplication.isPlaying = false;
#else
            Debug.Log("Detected USB Killer in build. Quitting application.");
            Application.Quit();
#endif
        }

        setPaperLocked(false);
    }

    private void setPaperLocked(bool locked)
    {
        foreach (GameObject paper in papers)
        {
            paper.GetComponent<XRGrabInteractable>().enabled = !locked;
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
