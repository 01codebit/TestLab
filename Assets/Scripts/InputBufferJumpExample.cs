using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

namespace UnityUtils
{
    public class InputBufferJumpExample : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField]
        private TimedInputBuffer jumpBufferInput = new();

        [SerializeField]
        private float jumpVelocity = 6f;

        [Header("Ground Settings")]
        [SerializeField]
        private float groundProbeDistance = 0.2f;

        [SerializeField]
        private LayerMask groundLayers = ~0;

        private Rigidbody rigidBody;
        private bool isGrounded;
        private InputAction jumpAction;

        protected void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        void Update()
        {
            if (jumpAction.IsPressed())
            {
                UnityEngine.Debug.Log("Jump pressed");
                jumpBufferInput.Set();
            }

            TryConsumeJumpBuffer();
        }

        void TryConsumeJumpBuffer()
        {
            if (!isGrounded || !jumpBufferInput.HasBuffer)
                return;

            rigidBody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            
            // var velocity = rigidBody.linearVelocity;
            // velocity.y = jumpVelocity;
            // rigidBody.linearVelocity = velocity;

            jumpBufferInput.Consume();
        }
    }
}
