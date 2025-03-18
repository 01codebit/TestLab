using System.Collections;
using System.Threading;
using TestLab.EventChannel.View;
using TestLab.Timing;
using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement

namespace TestLab.EventChannel
{
    public class TestInputSystem : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private TestInputSettingsSO settings;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private Transform anchor;

        // 2. These variables are to hold the Action references
        InputAction moveAction;
        InputAction sprintAction;
        InputAction jumpAction;
        bool jumping;

        InputAction clickAction;
        bool clickingEnabled = true;

        private Vector3 initialPosition;
        private Vector3 initialForward;
        private Quaternion initialRotation;

        private Rigidbody rigidBody;

        private GameObjectPool gameObjectPool;
        private int pooledObjects = 0;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialForward = transform.forward;

            rigidBody = GetComponent<Rigidbody>();

            // 3. Find the references to the "Move" and "Jump" actions
            moveAction = InputSystem.actions.FindAction("Move");
            sprintAction = InputSystem.actions.FindAction("Sprint");
            jumpAction = InputSystem.actions.FindAction("Jump");
            clickAction = InputSystem.actions.FindAction("Click");

            gameObjectPool = new GameObjectPool(itemPrefab);
        }

        private MinimalTimer minimalTimer;
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                return;
            }


            Vector2 moveValue = moveAction.ReadValue<Vector2>() * settings.Speed;
            if (sprintAction.IsPressed())
            {
                moveValue *= settings.Sprint;
            }
            
            transform.position += transform.forward * moveValue.y;
            transform.position += transform.right * moveValue.x;

            if (jumpAction.IsPressed() && !jumping)
            {
                rigidBody.AddForce(Vector3.up * settings.JumpForce, ForceMode.Impulse);
                jumping = true;
            }

            if (minimalTimer.IsCompleted && !clickingEnabled)
            {
                clickingEnabled = true;
            }

            if (clickAction.IsPressed() && clickingEnabled)
            {
                clickingEnabled = false;

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

                    var item = gameObjectPool.GetAnchoredObject(anchor);
                    item.name = $"PooledObject_#{pooledObjects}";
                    item.transform.position = hit.point + new Vector3(0, 0.5f, 0);
                    pooledObjects++;

                    StartCoroutine(ReleaseAfter(item, 3.0f));
                }

                // Timer t = new(new TimerCallback(_ => clickingEnabled = true), this, 1, 1000);
                //minimalTimer = MinimalTimer.Start(.3f);
                StartCoroutine(Wait(.3f));
            }
        }

        IEnumerator Wait(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            clickingEnabled = true;
        }

        IEnumerator ReleaseAfter(GameObject gameObject, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            gameObjectPool.ReleaseObject(gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Floor"))
            {
                jumping = false;
            }
        }
    }
}