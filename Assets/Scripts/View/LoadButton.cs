using Logging;
using UnityEngine;
using UnityEngine.UI;

namespace TestLab.EventChannel.View
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField] private NetworkEventChannelSO m_NetworkEventChannel;
        [SerializeField] private GameObject m_container;

        private Button _button;

        private void OnEnable()
        {
            m_container.SetActive(false);
            
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TriggerNetworkEvent);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(TriggerNetworkEvent);
        }

        private void TriggerNetworkEvent()
        {
            m_container.SetActive(true);
            ConditionalLogger.Log("[LoadButton.TriggerNetworkEvent] raise network event");
            m_NetworkEventChannel?.RaiseEvent();
        }
    }
}