using UnityEngine;

namespace ImprovedTimers
{
    public class CountdownTimerTest : MonoBehaviour
    {
        [SerializeField] float countdown;

        CountdownTimer countdownTimer;

        void Start()
        {
            countdownTimer = new CountdownTimer(countdown);
            countdownTimer.OnTimerStart += () => Debug.Log("countdownTimer started");
            countdownTimer.OnTimerStop += () => Debug.Log("countdownTimer stopped");
            countdownTimer.Start();
        }

        void Update()
        {
            if(countdownTimer.IsRunning)
            {
                Debug.Log($"[CountdownTimerTest.Update] countdown: {countdownTimer.CurrentTime}");
            }
        }

        private void OnDestroy()
        {
            countdownTimer.Dispose();
        }
    }
}