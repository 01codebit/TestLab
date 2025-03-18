using UnityEngine;

public class LabelController : MonoBehaviour
{
    [SerializeField] private LabelSpawner labelSpawner;

    private void Start()
    {
        labelSpawner.SpawnLabel(gameObject.name, transform.position);
    }
}