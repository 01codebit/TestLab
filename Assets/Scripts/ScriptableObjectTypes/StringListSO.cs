using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjectTypes
{
    [CreateAssetMenu(menuName = "Scriptable Object Values/StringListSO", fileName = "StringListSO")]
    public class StringListSO : ScriptableObject
    {
        public List<string> Values;
    }
}