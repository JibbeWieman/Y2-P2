using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float rotateSpeed = 6.0f;
    public GameObject minHand;      
    public GameObject hourHand;     

    void Update()
    {
        minHand.transform.Rotate(-rotateSpeed * Time.deltaTime, 0, 0, Space.Self);
        hourHand.transform.Rotate(-(rotateSpeed / 12) * Time.deltaTime, 0, 0, Space.Self);
    }
}