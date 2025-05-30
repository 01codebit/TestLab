using UnityEngine;

[CreateAssetMenu(fileName = "StatusSO", menuName = "Scriptable Objects/StatusSO")]
public class StatusSO : ScriptableObject
{
    [SerializeField] private string Name;
    
    [SerializeReference] private StatusSO Next;
    
    public StatusSO StepOver()
    {
        return Next;
    }
}
