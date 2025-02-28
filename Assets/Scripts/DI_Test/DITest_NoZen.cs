using UnityEngine;
using Zenject;

public class DITestNoZen : MonoBehaviour
{
    [SerializeField] private float m_angularSpeed = 1.0f;
    [SerializeField] private Transform m_tr;

    void Start()
    {
        Debug.Log("[DITestNoZen.Start]");
        if(m_tr==null) Debug.Log("m_tr is null");
    }

    void Update()
    {
        m_tr.Rotate(Vector3.up, Time.deltaTime * m_angularSpeed);
    }
}
