using UnityEngine;

public class TestDrawLineScript : MonoBehaviour
{
    [SerializeField] private Transform _rectStart;
    [SerializeField] private Transform _rectEnd;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Debug.DrawLine(_rectStart.position, _rectEnd.position, Color.red, 3.0f);
    }

    // Update is called once per frame
    private void Update()
    {
        // if (Input.GetKey(KeyCode.LeftControl))
        // {
        //     Debug.DrawLine(_rectStart.position, _rectEnd.position, Color.red, 3.0f);
        // }
    }
}
