using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LabelView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;

        public void SetLabelText(string t)
        {
            label.text = t;
        }
    }
}