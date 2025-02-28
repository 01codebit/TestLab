using UnityEngine;

public class SingletonTest : MonoBehaviour
{
    private static SingletonTest Instance;

    private void OnEnable()
    {
        Instance = this;
    }

    public static SingletonTest GetSingleton()
    {
        return Instance;
    }

    public void SingletonTestMethod()
    {
        Debug.Log("[SingletonTest] Hello from Singleton!");
    }
}
