using UnityEngine;

public class LabelBinder : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private WorldSpaceUIDocument uiDocument;

    void Start()
    {
        uiDocument.SetLabelText("Label", target.name);
    }
}
