using System;
using UnityEngine;

namespace TestLab.EventChannel
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraTransform;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        
        private void Start()
        {
            initialPosition = cameraTransform.position;
            initialRotation = cameraTransform.rotation;
        }
        
        private void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                var dv = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
                dv = dv.normalized * 0.1f;
                cameraTransform.position += dv;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                var dv = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z);
                dv = dv.normalized * 0.1f;
                cameraTransform.position -= dv;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                var rotatedVector = Quaternion.Euler(0,-.5f,0) * cameraTransform.forward;
                cameraTransform.forward = rotatedVector;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                var rotatedVector = Quaternion.Euler(0,.5f,0) * cameraTransform.forward;
                cameraTransform.forward = rotatedVector;
            }
            else if (Input.GetKey(KeyCode.R))
            {
                cameraTransform.position = initialPosition;
                cameraTransform.rotation = initialRotation;
            }
        }
    }
}