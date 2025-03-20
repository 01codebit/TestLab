using System;
using System.Collections;
using Unity.Transforms;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 4f;
    
    private InputSystem_Actions controls;
    private Coroutine zoomCoroutine;
    private Transform cameraTransform;
    
    private void Awake()
    {
        controls = new InputSystem_Actions();
        cameraTransform = Camera.main.transform;
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
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }
    
    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }
    
    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    private IEnumerator ZoomDetection()
    {
        float prevDistance = 0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(
                controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), 
                controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>()
            );

            if (distance > prevDistance)
            {
                // zoom out
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z -= 1;
                cameraTransform.position =
                    Vector3.Slerp(cameraTransform.position, 
                        targetPosition, 
                        Time.deltaTime * cameraSpeed);
            }
            else if (distance < prevDistance)
            {
                // zoom in
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.z += 1;
                cameraTransform.position =
                    Vector3.Slerp(cameraTransform.position, 
                        targetPosition, 
                        Time.deltaTime * cameraSpeed);
            }
            
            prevDistance = distance;

            yield return null;
        }
    }
}
