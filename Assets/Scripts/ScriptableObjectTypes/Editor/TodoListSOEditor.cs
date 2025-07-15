using UnityEditor;
using UnityEngine;

namespace Channels.Editor
{
    [CustomEditor(typeof(TodoListSO))]
    public class TodoListSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            TodoListSO so = (TodoListSO)target;

            if (GUILayout.Button("Notify Change"))
            {
                so.NotifyChange();
            }

            if (GUILayout.Button("Notify Clear"))
            {
                so.NotifyClear();
            }

            if (GUILayout.Button("Clear"))
            {
                so.ClearItems();
            }

            if (GUILayout.Button("Clear and notify"))
            {
                so.ClearItems(true);
            }
        }
    }
}