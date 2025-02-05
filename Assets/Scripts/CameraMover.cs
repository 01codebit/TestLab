using UnityEngine;

namespace TestLab.EventChannel
{
    public class CameraMover : MonoBehaviour
    {
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private void Start()
        {
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                return;
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var dv = new Vector3(transform.forward.x, 0, transform.forward.z);
                dv = dv.normalized * 0.1f;
                transform.position += dv;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                var dv = new Vector3(transform.forward.x, 0, transform.forward.z);
                dv = dv.normalized * 0.1f;
                transform.position -= dv;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                var rotatedVector = Quaternion.Euler(0, -.5f, 0) * transform.forward;
                transform.forward = rotatedVector;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                var rotatedVector = Quaternion.Euler(0, .5f, 0) * transform.forward;
                transform.forward = rotatedVector;
            }
        }
    }
}