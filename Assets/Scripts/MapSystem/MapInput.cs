using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement
using UnityEngine.Assertions;

namespace MapSystem
{
    public class MapInput : MonoBehaviour
    {
        [SerializeField] private GameObject tile;
        [SerializeField] private float speedScale;

        private TileDownloader tileDownloader;
        private MeshFilter meshFilter;

        // 2. These variables are to hold the Action references
        InputAction moveAction;
        InputAction resetAction;

        private int startingX;
        private int startingY;
        private int startingZ;

        private Vector3 startingPosition;
        private Vector3 startingSize;


        private void Start()
        {
            // 3. Find the references to the "Move" and "Jump" actions
            moveAction = InputSystem.actions.FindAction("Move");
            resetAction = InputSystem.actions.FindAction("Reset");

            tileDownloader = tile.GetComponent<TileDownloader>();
            Assert.IsNotNull(tileDownloader);

            meshFilter = tile.GetComponent<MeshFilter>();
            Assert.IsNotNull(tileDownloader);
            startingSize = meshFilter.sharedMesh.bounds.size;

            startingX = tileDownloader.CoordX;
            startingY = tileDownloader.CoordY;
            startingZ = tileDownloader.ZoomLevel;

            startingPosition = tile.transform.position;
        }

        private void Update()
        {
            if (resetAction.IsPressed())
            {
                tileDownloader.CoordX = startingX;
                tileDownloader.CoordY = startingY;
                tileDownloader.ZoomLevel = startingZ;
                tile.transform.position = startingPosition;

                return;
            }


            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            // if(moveValue.x > 0) tileDownloader.CoordX += 1;
            // if(moveValue.x < 0) tileDownloader.CoordX -= 1;
            // if(moveValue.y > 0) tileDownloader.CoordY += 1;
            // if(moveValue.y < 0) tileDownloader.CoordY -= 1;

            tile.transform.position += new Vector3(moveValue.x * speedScale, 0, moveValue.y * speedScale);

            Vector3 pos = tile.transform.position;

            if (moveValue.x > 0 && tile.transform.position.x > startingSize.x/2)
            {
                pos.x -= startingSize.x;
            }
            else if (moveValue.x < 0 && tile.transform.position.x < -startingSize.x/2)
            {
                pos.x += startingSize.x;
            }
            
            if (moveValue.y > 0 && tile.transform.position.z > startingSize.z)
            {
                pos.z -= startingSize.z * 2;
            }
            else if (moveValue.y < 0 && tile.transform.position.z < -startingSize.z)
            {
                pos.z += startingSize.z * 2;
            }

            if (pos.x != tile.transform.position.x || pos.z != tile.transform.position.z)
                tile.transform.position = pos;
        }
    }
}