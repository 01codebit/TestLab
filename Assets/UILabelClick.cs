using UnityEngine;

public class UILableClick : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private InputSystem_Actions controls;
    
    private void Awake()
    {
        controls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Start()
    {
        // var isPressed = controls.UI.Click.IsPressed();
        controls.UI.Click.started += _ => UILabelClicked();
        controls.UI.Click.canceled += _ => UILabelClicked();
    }

    private void UILabelClicked()
    {
        Debug.Log("[UILabel.UILabelClicked] clicked");
    }
}
