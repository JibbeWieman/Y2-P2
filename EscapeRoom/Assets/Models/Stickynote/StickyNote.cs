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


    public float CalculateContrastRatio()
    {
        return ContrastRatio(materialColor, textColor);
    }

    private float ContrastRatio(Color bg, Color text)
    {
        float bgLuminance = RelativeLuminance(bg);
        float textLuminance = RelativeLuminance(text);
        float lighter = Mathf.Max(bgLuminance, textLuminance);
        float darker = Mathf.Min(bgLuminance, textLuminance);
        return (lighter + 0.05f) / (darker + 0.05f);
    }

    private float RelativeLuminance(Color color)
    {
        float r = (color.r <= 0.03928f) ? (color.r / 12.92f) : Mathf.Pow((color.r + 0.055f) / 1.055f, 2.4f);
        float g = (color.g <= 0.03928f) ? (color.g / 12.92f) : Mathf.Pow((color.g + 0.055f) / 1.055f, 2.4f);
        float b = (color.b <= 0.03928f) ? (color.b / 12.92f) : Mathf.Pow((color.b + 0.055f) / 1.055f, 2.4f);
        return 0.2126f * r + 0.7152f * g + 0.0722f * b;
    }
}
