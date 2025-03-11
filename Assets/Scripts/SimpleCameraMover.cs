using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization; // 1. The Input System "using" statement

namespace TestLab.EventChannel
{
    public class SimpleCameraMover : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float speed;
        [SerializeField] private float angularSpeed;

        // 2. These variables are to hold the Action references
        InputAction moveAction;
        
        private Vector3 initialPosition;
        private Vector3 initialForward;
        private Quaternion initialRotation;
        
        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialForward = transform.forward;
            
            // 3. Find the references to the "Move" and "Jump" actions
            moveAction = InputSystem.actions.FindAction("Move");
        }
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                return;
            }
            
            var moveValue = moveAction.ReadValue<Vector2>() * speed;
            transform.position += transform.forward * moveValue.y;
            transform.position += transform.right * moveValue.x;
        }
    }
}