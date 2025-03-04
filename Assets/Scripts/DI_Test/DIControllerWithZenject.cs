using TMPro;
using UnityEngine;
using Zenject;

public class DIControllerWithZenject : MonoBehaviour
{
    [SerializeField] private float m_angularSpeed = 1.0f;

    private TextMeshProUGUI m_label;
    private Transform m_transform;

    [Inject]
    private void Inject(
        [Inject(Id ="Label")] TextMeshProUGUI label, 
        [Inject(Id ="Cube")] Transform transform
    )
    {
        m_label = label;
        m_transform = transform;
    }

    private void Start()
    {
        if (m_transform == null) Debug.Log("[DIControllerWithZenject.Start] Error: m_transform is null");
        
        if (m_label != null)
        {
            m_label.SetText("[DIControllerWithZenject]");
        }
        else
        {
            Debug.Log("[DIControllerWithZenject.Start] Error: m_label is null");
        }
    }

    private void Update()
    {
        var new_angle = Mathf.Round(Time.deltaTime * m_angularSpeed * 100f) / 100f;
        m_transform?.Rotate(Vector3.up, new_angle);
        
        var angle = Mathf.Round(m_transform.rotation.eulerAngles.y * 100f) / 100f;
        m_label?.SetText($"[DIControllerWithZenject]\nangle: {angle}");
    }
}
