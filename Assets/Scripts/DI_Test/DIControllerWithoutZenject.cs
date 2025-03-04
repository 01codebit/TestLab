using TMPro;
using UnityEngine;

public class DIControllerWithoutZenject : MonoBehaviour
{
    [SerializeField] private float m_angularSpeed = 1.0f;
    
    [SerializeField] private Transform m_transform;
    [SerializeField] private TextMeshProUGUI m_label;
    
    private void Start()
    {
        if (m_transform == null) Debug.Log("[DIControllerWithoutZenject.Start] Error: m_transform is null");
        
        if (m_label != null)
        {
            m_label.SetText("[DIControllerWithoutZenject]");
        }
        else
        {
            Debug.Log("[DIControllerWithoutZenject.Start] Error: m_label is null");            
        }
    }

    private void Update()
    {
        var new_angle = Mathf.Round(Time.deltaTime * m_angularSpeed * 100f) / 100f;
        m_transform?.Rotate(Vector3.up, new_angle);
        
        var angle = Mathf.Round(m_transform.rotation.eulerAngles.y * 100f) / 100f;
        m_label?.SetText($"[DIControllerWithoutZenject]\nangle: {angle}");
    }
}
