using UnityEditor;
using UnityEngine;

namespace Channels.Editor
{
    [CustomEditor(typeof(VoidEventChannelSO))]
    public class VoidEventChannelSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            VoidEventChannelSO myGameEvent = (VoidEventChannelSO)target;

            if (GUILayout.Button("RaiseEvent()"))
            {
                myGameEvent.RaiseEvent();
            }
        }
    }
}