using UnityEngine;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;

namespace UnityUtils
{
    public class InputBufferJumpExample : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField]
        private TimedInputBuffer jumpBuffer = new();

        [Header("Jump")]
        [SerializeField]
        private float jumpVelocity = 6f;

        [Header("Ground Check")]
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

        private bool IsJumpPressedThisFrame() => jumpAction.IsPressed();
        
        private void Update()
        {
            if (IsJumpPressedThisFrame())
            {
                jumpBuffer.Set();
            }

            TryConsumeBufferedJump();
        }

        private void TryConsumeBufferedJump()
        {
            if (!isGrounded || !jumpBuffer.HasBuffer)
            {
                Debug.Log($"[InputBufferJumpExample.TryConsumeBufferedJump] isGrounded: {isGrounded} - jumpBufferInput.HasBuffer: {jumpBuffer.HasBuffer}");
                return;
            }

            Debug.Log("[InputBufferJumpExample.TryConsumeBufferedJump]");

            DoJump();
            
            // var velocity = rigidBody.linearVelocity;
            // velocity.y = jumpVelocity;
            // rigidBody.linearVelocity = velocity;

            jumpBuffer.Consume();
        }

        private void DoJump()
        {
            // if (isJumping) return;
            
            rigidBody.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            isGrounded = false;
        }
    }
}
