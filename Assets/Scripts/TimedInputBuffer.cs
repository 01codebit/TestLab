using System;
using UnityEngine;

namespace UnityUtils
{
    [Serializable]
    public class TimedInputBuffer : TimedInputBuffer<bool>
    {
        public void Set() => Set(true);

        public bool TryConsume() => TryConsume(out _);
    }

    [Serializable]
    public class TimedIntInputBuffer : TimedInputBuffer<int> { }

    [Serializable]
    public class TimedFloatInputBuffer : TimedInputBuffer<float> { }

    [Serializable]
    public class TimedVector2InputBuffer : TimedInputBuffer<Vector2> { }

    [Serializable]
    public class TimedInputBuffer<T>
    {
        [SerializeField, Min(0f)]
        private float holdTime = 0.12f;

        [SerializeField]
        private T bufferedValue;

        float expiryTime = float.NegativeInfinity;
        bool hasBufferedInput;

        public bool HasBuffer => hasBufferedInput && Time.time < expiryTime;
        public float HoldTime => holdTime;
        public T Value => bufferedValue;

        public void Set(T value)
        {
            bufferedValue = value;
            hasBufferedInput = true;
            expiryTime = Time.time + holdTime;
        }

        public void Consume()
        {
            hasBufferedInput = false;
            expiryTime = float.NegativeInfinity;
            bufferedValue = default;
        }

        public bool TryConsume(out T value)
        {
            if (HasBuffer)
            {
                value = bufferedValue;
                Consume();
                return true;
            }

            value = default;
            return false;
        }

        public void SetHoldTime(float seconds) => holdTime = Mathf.Max(0f, seconds);
    }
}
