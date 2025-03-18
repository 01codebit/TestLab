using UnityEngine;
using UnityEngine.Pool;

public class LabelSpawner : MonoBehaviour
{
    [SerializeField] private WorldSpaceUIDocument uiDocumentPrefab;

    private IObjectPool<WorldSpaceUIDocument> _uiDocumentPool;
    private const string KLabelName = "Label";

    void Awake() => _uiDocumentPool = new ObjectPool<WorldSpaceUIDocument>(
        Create,
        OnTake,
        OnReturn,
        OnDestroy
    );

    private WorldSpaceUIDocument Create() => Instantiate(uiDocumentPrefab, transform, true);
    private static void OnTake(WorldSpaceUIDocument uiDocument) => uiDocument.gameObject.SetActive(true);
    private static void OnReturn(WorldSpaceUIDocument uiDocument) => uiDocument.gameObject.SetActive(false);

    private static void OnDestroy(WorldSpaceUIDocument uiDocument)
    {
        if (uiDocument == null) return;
        Destroy(uiDocument.gameObject);
    }

    public void SpawnLabel(string text, Vector3 worldPosition)
    {
        var instance = _uiDocumentPool.Get();
        instance.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
        instance.SetLabelText(KLabelName, text);
    }
}