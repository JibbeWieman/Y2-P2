using UnityEditor;

[CustomEditor(typeof(StickyNote))]
public class StickyNoteEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StickyNote stickyNote = (StickyNote)target;

        DrawDefaultInspector();

        float contrastRatio = stickyNote.CalculateContrastRatio();

        bool passesAA = contrastRatio >= 4.5f;
        bool passesAALarge = contrastRatio >= 3.0f;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Contrast Checker", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"Contrast Ratio: {contrastRatio:F2}");

        EditorGUILayout.LabelField($"Passes AA (Normal Text): {passesAA}");
        EditorGUILayout.LabelField($"Passes AA (Large Text): {passesAALarge}");

        if (!passesAA)
        {
            EditorGUILayout.HelpBox("This text does not meet AA guidelines for accessibility.", MessageType.Warning);
        }
    }
}
