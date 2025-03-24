using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UILabelBinder : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string labelName;

    private Label label;
    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
        label = uiDocument.rootVisualElement.Q<Label>(labelName); 
        
        label.text = target.name;
    }

    private void Update()
    {
        if (target.transform.hasChanged)
        {
            var uiPos = camera.WorldToScreenPoint(target.transform.position);
            
            // Debug.Log($"[UILabelBinder.Update] Screen: width: {Screen.width}, height: {Screen.height}");
            // Debug.Log($"[UILabelBinder.Update] uiPos: ({uiPos.x}, {uiPos.y}, {uiPos.z})");

            var dh = 100;//label.style.height.value.value * 2f;
            label.style.bottom = uiPos.y + dh;
            var dw = label.style.width.value.value * .5f;
            label.style.left = uiPos.x - dw;
        }
    }
}
