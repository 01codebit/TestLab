using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement

namespace TestLab.EventChannel
{
    public class TestInputSystem : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        
        // 2. These variables are to hold the Action references
        InputAction moveAction;
        InputAction sprintAction;
        InputAction lookAction;
        InputAction clickAction;
        
        private Vector3 initialPosition;
        private Vector3 initialForward;
        private Quaternion initialRotation;

        private float speed = 0.1f;
        private float sprint = 3.0f;
        
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialForward = transform.forward;
            
            // 3. Find the references to the "Move" and "Jump" actions
            moveAction = InputSystem.actions.FindAction("Move");
            sprintAction = InputSystem.actions.FindAction("Sprint");
            lookAction = InputSystem.actions.FindAction("Look");
            clickAction = InputSystem.actions.FindAction("Click");
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                return;
            }


            Vector2 moveValue = moveAction.ReadValue<Vector2>() * speed;
            if (sprintAction.IsPressed())
            {
                moveValue *= sprint;
            }
            
            transform.position += transform.forward * moveValue.y;
            transform.position += transform.right * moveValue.x;
            
            // Vector2 lookValue = lookAction.ReadValue<Vector2>();

            if (clickAction.IsPressed())
            {
                RaycastHit hit;
                Debug.Log($"Input.mousePosition: {Input.mousePosition}");

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                Debug.Log($"forward: {transform.forward}");

                if (Physics.Raycast(ray, out hit))
                {
                    // Do something with the object that was hit by the raycast.
                    Vector3 newForward = hit.point - transform.position;
                    newForward.y = initialForward.y;
                    transform.forward = newForward;
                }
            }
        }
    }
}