using UnityEngine;
using UnityEngine.InputSystem; // 1. The Input System "using" statement
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace TestLab.EventChannel
{
    public class CameraMover : MonoBehaviour
    {
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private float speed = 0.5f;
        
        // 2. These variables are to hold the Action references
        InputAction resetAction;
        InputAction moveAction;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            
            // 3. Find the references to the actions
            resetAction = InputSystem.actions.FindAction("Reset");
            moveAction = InputSystem.actions.FindAction("Move");
        }

        private void Awake()
        {
            // Enable enhanced touch support if not already
            if (!EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Enable();
        }

        private void Update()
        {
            if (resetAction.IsPressed())
            {
                Debug.Log("[CameraMover.Update] Reset");
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                return;
            }
            
            var moveValue = moveAction.ReadValue<Vector2>() * speed;
            var forward = Vector3.Normalize(new Vector3(transform.forward.x, 0f, transform.forward.z));
            transform.Translate(forward * moveValue.y, Space.World);
            
            var rotatedVector = Quaternion.Euler(0, moveValue.x, 0) * transform.forward;
            transform.forward = rotatedVector;


            // if (Input.GetKey(KeyCode.UpArrow))
            // {
            //     var dv = new Vector3(transform.forward.x, 0, transform.forward.z);
            //     dv = dv.normalized * 0.1f;
            //     transform.position += dv;
            // }
            // else if (Input.GetKey(KeyCode.DownArrow))
            // {
            //     var dv = new Vector3(transform.forward.x, 0, transform.forward.z);
            //     dv = dv.normalized * 0.1f;
            //     transform.position -= dv;
            // }
            //
            // if (Input.GetKey(KeyCode.LeftArrow))
            // {
            //     var rotatedVector = Quaternion.Euler(0, -.5f, 0) * transform.forward;
            //     transform.forward = rotatedVector;
            // }
            // else if (Input.GetKey(KeyCode.RightArrow))
            // {
            //     var rotatedVector = Quaternion.Euler(0, .5f, 0) * transform.forward;
            //     transform.forward = rotatedVector;
            // }
        }
        
        public void Scroll(InputAction.CallbackContext context)
        {
            Debug.Log("[CameraMover.Scroll]");
            if (context.phase != InputActionPhase.Performed)
                return;
    
            float scrollDistance = context.ReadValue<Vector2>().y;
            Zoom(scrollDistance);
        }
        
        private void Zoom(float distance)
        {
            distance *= 0.01f;
            transform.localScale += new Vector3(distance, distance, distance);
        }
    }
}