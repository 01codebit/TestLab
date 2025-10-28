using UnityEngine;
// 1. The Input System "using" statement
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace TestLab.EventChannel
{
    public class CameraMover : MonoBehaviour
    {
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        [SerializeField] private float speed = 0.5f;
        
        // 2. These variables are to hold the Action references
        InputAction resetAction;
        InputAction moveAction;
        // InputAction lookAction;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            
            // 3. Find the references to the actions
            resetAction = InputSystem.actions.FindAction("UI/Reset");
            resetAction.performed += HandleReset; 

            moveAction = InputSystem.actions.FindAction("Player/Move");
            // moveAction.performed += HandleMove;
            
            // lookAction = InputSystem.actions.FindAction("Player/Look");
            // lookAction.performed += HandleLook;
        }

        private void Awake()
        {
            // Enable enhanced touch support if not already
            if (!EnhancedTouchSupport.enabled)
                EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            resetAction.performed -= HandleReset;
            // moveAction.performed -= HandleMove;
            // lookAction.performed -= HandleLook;
        }
        
        private void HandleReset(InputAction.CallbackContext ctx)
        {
            Debug.Log("[CameraMover.Update] Reset");
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }

        private void HandleMove(InputAction.CallbackContext ctx)
        {
            var moveValue = moveAction.ReadValue<Vector2>() * speed;
            // var forward = Vector3.Normalize(new Vector3(transform.forward.x, 0f, transform.forward.z));
            // transform.Translate(forward * moveValue.y, Space.Self);

            transform.Translate(transform.forward * moveValue.y, Space.Self);
            var rotatedVector = Quaternion.Euler(0, moveValue.x, 0) * transform.forward;
            transform.forward = rotatedVector;
        }
        
        private void HandleLook(InputAction.CallbackContext ctx)
        {
            var lookValue = ctx.ReadValue<Vector2>();
            //Debug.Log($"[{name}] lookValue: {lookValue}");
            
            var rotatedVector = Quaternion.Euler(-lookValue.y, lookValue.y, 0) * transform.forward;
            transform.forward = rotatedVector;
        }

        private void Update()
        {
            if (moveAction.inProgress)
            {
                var moveValue = moveAction.ReadValue<Vector2>() * speed;
                var forward = Vector3.Normalize(new Vector3(transform.forward.x, 0f, transform.forward.z));
                transform.Translate(forward * moveValue.y, Space.World);                
                
                var rotatedVector = Quaternion.Euler(0, moveValue.x, 0) * transform.forward;
                transform.forward = rotatedVector;

            }

            // if (lookAction.inProgress)
            // {
            //     var lookValue = lookAction.ReadValue<Vector2>();
            //     var rotatedVector = Quaternion.Euler(-lookValue.y, lookValue.y, 0) * transform.forward;
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