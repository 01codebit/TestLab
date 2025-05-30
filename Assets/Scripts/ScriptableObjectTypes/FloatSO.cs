using UnityEngine;

namespace ScriptableObjectTypes
{
    [CreateAssetMenu(menuName = "Scriptable Object Values/FloatSO", fileName = "FloatSO")]
    public class FloatSO : ScriptableObject
    {
        public float Value;
    }
}