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
            label.style.bottom = uiPos.y;
            label.style.left = uiPos.x;
        }
    }
}
