using UnityEngine;

namespace DOTSTest
{
    public class NotDotsBallSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject Prefab;
        [SerializeField] private int Columns;
        [SerializeField] private int Rows;

        private void Start()
        {   
            var position = gameObject.transform.position;
            var origin = new Vector3(position.x - Columns/2, 1.0f, position.z - Rows/2);

            for(int i=0; i<Columns*Rows; i++)
            {
                var ball = Instantiate(Prefab);
                var pos = new Vector3(origin.x + i%Columns, origin.y, origin.z + i/Columns);
                var tr = ball.GetComponent<Transform>();
                tr.position = pos;
            }
        }
    }
}