using UnityEngine;

namespace DefaultNamespace
{
    public class Object2DLabel : MonoBehaviour
    {
        [SerializeField] private GameObject labeledObject;
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private GameObject labelPrefab;
        [SerializeField] private float labelHeight;

        private LabelView labelView;
        private Camera mainCamera;
        private GameObject label;
        
        private void Start()
        {
            var objectName = labeledObject.name;
            labelView = labelPrefab.GetComponent<LabelView>();
            labelView.SetLabelText(objectName);

            mainCamera = Camera.main;
            label = Instantiate(labelPrefab);
            label.transform.SetParent(uiCanvas.transform);

            SetLabel();
        }

        private void Update()
        {
            if (transform.hasChanged)
            {
                SetLabel();
            }
        }

        private void SetLabel()
        {
            var objectPos = labeledObject.transform.position;
            objectPos += new Vector3(0, labelHeight, 0);
            var screenPos = mainCamera.WorldToScreenPoint(objectPos);
            label.transform.position = screenPos;
            // lbl.transform.localScale = transform.localScale;
        }
    }
}