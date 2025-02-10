using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

namespace MapSystem
{
    public class TileDownloader : MonoBehaviour
    {
        [SerializeField] private string BaseUrl = "https://tile.openstreetmap.org/Z/X/Y.png";
        [SerializeField] private Renderer _renderer;
        [SerializeField] private bool RoundRobin;

        [SerializeField] private int DeltaX;
        [SerializeField] private int DeltaY;


        public int CoordX = 0;
        public int CoordY = 0;
        public int ZoomLevel = 0;

        private int prevCoordX;
        private int prevCoordY;
        private int prevZoomLevel;

        void Start()
        {
            prevCoordX = CoordX;
            prevCoordY = CoordY;
            prevZoomLevel = ZoomLevel;

            CheckValues();
            StartCoroutine(GetTileTexture());
        }

        void CheckZoom()
        {
            if(ZoomLevel<0) ZoomLevel = 0;
            else if(ZoomLevel>18) ZoomLevel = 18;

            // if(prevZoomLevel>ZoomLevel)
            // {
            //     CoordX /= 2;
            //     CoordY /= 2;
            // }
            // else if(prevZoomLevel<ZoomLevel)
            // {
            //     CoordX *= 2;
            //     CoordY *= 2;
            // }
        }

        void CheckValues()
        {
            int total = (int)Math.Pow(4, ZoomLevel);
            int max = (int)Math.Sqrt(total) - 1;

            if (RoundRobin)
            {
                if (CoordX > max)
                    CoordX = 0;
                else if (CoordX < 0)
                    CoordX = max;

                if (CoordY < 0)
                    CoordY = max;
                else if (CoordY > max)
                    CoordY = 0;
            }
            else
            {
                if (CoordX > max)
                    CoordX = max;
                else if (CoordX < 0)
                    CoordX = 0;

                if (CoordY < 0)
                    CoordY = 0;
                else if (CoordY > max)
                    CoordY = max;
            }
        }

        void Update()
        {
            if(prevZoomLevel != ZoomLevel)
            {
                CheckZoom();
                CheckValues();
                StartCoroutine(GetTileTexture());
            }

            if (CoordX != prevCoordX
                || CoordY != prevCoordY
            )
            {
                CheckValues();
                StartCoroutine(GetTileTexture());
            }

        }

        private string PrepareUrl()
        {
            string url = BaseUrl.Replace("X", $"{CoordX}");
            url = url.Replace("Y", $"{CoordY}");
            url = url.Replace("Z", $"{ZoomLevel}");

            return url;
        }

        IEnumerator GetTileTexture()
        {
            string url = PrepareUrl();
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Texture tileTexture = DownloadHandlerTexture.GetContent(request);
                _renderer.sharedMaterial.mainTexture = tileTexture;
            }
        }
    }
}