using UnityEngine;

public class SingletonTestClient : MonoBehaviour
{
    void Start()
    {
        SingletonTest.GetSingleton().SingletonTestMethod();
    }
}
