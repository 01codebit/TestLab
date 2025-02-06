using UnityEngine;

namespace TestLab.EventChannel
{
    [CreateAssetMenu(menuName = "TestInput/TestInputSettings", fileName = "TestInputSettings")]
    public class TestInputSettingsSO : ScriptableObject
    {
        [SerializeField] private float speed = 0.1f;
        [SerializeField] private float sprint = 3.0f;
        [SerializeField] private float jumpForce = 2.0f;

        public float Speed => speed;
        public float Sprint => sprint;
        public float JumpForce => jumpForce;
    }
}