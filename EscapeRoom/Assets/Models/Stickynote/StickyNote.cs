using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[ExecuteAlways]
public class StickyNote : MonoBehaviour
{
    // Serializable field to set the color from the Inspector, with a default value of FFE79D
    [SerializeField]
    private Color materialColor = new Color();

    [SerializeField]
    private Color textColor = Color.white;


    [SerializeField]
    private string text = "Sample";

    private Renderer objectRenderer;

    private Material instanceMaterial;

    private TextMeshPro textObj;

    // Awake is called before Start, to initialize the default color
    private void Awake()
    {
        if (ColorUtility.TryParseHtmlString("#FFE79D", out Color defaultColor))
        {
            if (materialColor == new Color(0, 0, 0, 0))
            {
                materialColor = defaultColor;
            }
        }

        UpdateMaterialColor();
        UpdateText();
    }

    private void OnValidate()
    {
        UpdateMaterialColor();
        UpdateText();
    }

    private void UpdateMaterialColor()
    {
        objectRenderer = GetComponentInChildren<Renderer>();

        if (objectRenderer != null)
        {
            if (instanceMaterial == null)
            {
                instanceMaterial = new Material(objectRenderer.sharedMaterial);
                objectRenderer.material = instanceMaterial;
            }

            instanceMaterial.color = materialColor;
        }
        else
        {
            Debug.LogWarning("No Renderer found on this GameObject.");
        }
    }

    private void UpdateText()
    {
        if (textObj == null)
        {
            textObj = GetComponentInChildren<TextMeshPro>();
        }

        if (textObj != null)
        {
            textObj.text = text;
            textObj.faceColor = textColor;
        }
        else
        {
            Debug.LogWarning("No TextMeshPro component found on this GameObject.");
        }
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying && instanceMaterial != null)
        {
            DestroyImmediate(instanceMaterial);
        }
    }
}
