using UnityEngine;
using UnityEngine.InputSystem;  // 1. The Input System "using" statement

namespace MapSystem
{
    public class MapInput : MonoBehaviour
    {
        [SerializeField] private TileDownloader tileDownloader;

        // 2. These variables are to hold the Action references
        InputAction moveAction;

        private int startingX;
        private int startingY;
        private int startingZ;

        private void Start()
        {
            // 3. Find the references to the "Move" and "Jump" actions
            moveAction = InputSystem.actions.FindAction("Move");

            startingX = tileDownloader.CoordX;
            startingY = tileDownloader.CoordY;
            startingZ = tileDownloader.ZoomLevel;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.R))
            {
                tileDownloader.CoordX = startingX;
                tileDownloader.CoordY = startingY;
                tileDownloader.ZoomLevel = startingZ;
                
                return;
            }


            Vector2 moveValue = moveAction.ReadValue<Vector2>();
            if(moveValue.x > 0) tileDownloader.CoordX += 1;
            if(moveValue.x < 0) tileDownloader.CoordX -= 1;
            if(moveValue.y > 0) tileDownloader.CoordY += 1;
            if(moveValue.y < 0) tileDownloader.CoordY -= 1;
       }
    }
}