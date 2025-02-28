using TMPro;
using UnityEngine;
using Zenject;

public class DITest : MonoBehaviour
{
    [SerializeField] private float m_angularSpeed = 1.0f;

    private TextMeshProUGUI m_label;
    private Transform m_tr;

    [Inject]
    private void Inject(
        [Inject(Id ="Label")] TextMeshProUGUI l, 
        [Inject(Id ="Cube")] Transform c
    )
    {
        m_label = l;
        m_tr = c;
    }

    void Start()
    {
        Debug.Log("[DITest.Start]");
        if (m_tr == null) Debug.Log("m_tr is null");

        m_label.SetText("Ciao!");
    }

    void Update()
    {
        m_tr.Rotate(Vector3.up, Time.deltaTime * m_angularSpeed);
    }

}
