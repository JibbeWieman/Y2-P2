using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckKey : MonoBehaviour
{
    [SerializeField]
    private Collider checkKeyArea;

    [SerializeField]
    private GameObject requiredKey;

    private GameObject insertedObject;

    private void Start()
    {
        checkKeyArea = GetComponent<Collider>();
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
