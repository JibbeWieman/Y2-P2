using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookColor : MonoBehaviour
{
    [SerializeField]
    private Color m_Color = new Color();

    private Renderer objectRenderer;
    private Material objectMaterial;

    private void OnValidate()
    {
       objectRenderer = GetComponent<Renderer>();

        if (objectRenderer != null)
        {
            if (objectRenderer.sharedMaterial == null)
            {
                return;
            }
            if (objectMaterial == null) 
            {
                objectMaterial = new Material(objectRenderer.sharedMaterial);
                objectRenderer.material = objectMaterial;   
            }
            objectMaterial.color = m_Color;
        }
    }

}
