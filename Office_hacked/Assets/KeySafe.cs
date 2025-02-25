using UnityEngine;

public class KeySafe : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        
    }

    
    void Unlock()
    {
        rb.useGravity = false;
    }
}
