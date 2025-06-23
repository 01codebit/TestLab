using TestLab.EventChannel;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VoidEventChannelSO))]
public class VoidEventChannelSOEditor : Editor
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