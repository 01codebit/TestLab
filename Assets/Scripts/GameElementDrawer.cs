#if UNITY_EDITOR

using UnityEditor;
using UnityEngine.SceneManagement;

/*
[CustomPropertyDrawer(typeof(GameElement))]
public class GameElementDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var descRect = new Rect(position.x, position.y, 30, position.height);
        var prefabRect = new Rect(position.x + 35, position.y, 50, position.height);

        // Draw fields - pass GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(descRect, property.FindPropertyRelative("Description"), GUIContent.none);
        EditorGUI.PropertyField(prefabRect, property.FindPropertyRelative("Prefab"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
*/

[CustomEditor(typeof(GameElement))]
[CanEditMultipleObjects]
public class GameElementEditor : Editor 
{
    SerializedProperty description;
    SerializedProperty prefab;

    void OnEnable()
    {
        description = serializedObject.FindProperty("Description");
        prefab = serializedObject.FindProperty("Prefab");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(prefab);
        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(Scene))]
[CanEditMultipleObjects]
public class SceneEditor : Editor 
{
    SerializedProperty name;

    void OnEnable()
    {
        name = serializedObject.FindProperty("name");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(name);
        serializedObject.ApplyModifiedProperties();
    }
}

#endif