using System.Threading.Tasks;
using Logging;
using UnityEngine;

namespace Channels
{
    [CreateAssetMenu(menuName = "Events/Handle Event", fileName = "HandleEvent")]
    public class HandleEventSO : ScriptableObject
    {
        [SerializeField] private Material idleMaterial;
        [SerializeField] private Material activatedMaterial;
        
        private GameObject gameObject;
        private MeshRenderer renderer;

        public void SetGameObject(GameObject go)
        {
            gameObject = go;
        }
        
        public async void HandleEvent()
        {
            ConditionalLogger.Log("[HandleEventSO.HandleEvent]");

            renderer = gameObject.GetComponent<MeshRenderer>(); 
            
            if (gameObject != null)
            {
                renderer.material = activatedMaterial;
                await ResetMaterial();
            }
        }

        private async Task ResetMaterial()
        {
            await Task.Delay(2000);
            renderer.material = idleMaterial;
        }
    }
}