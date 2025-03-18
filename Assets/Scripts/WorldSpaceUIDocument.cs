using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

// https://discussions.unity.com/t/uitoolkit-world-space-support-status/887441/22
// https://gist.githubusercontent.com/katas94/7b220a591215efc36110860a0b1125eb
public class WorldSpaceUIDocument : MonoBehaviour
{
    #region Fields

    private const string KTransparentShader = "Unlit/Transparent";
    private const string KTextureShader = "Unlit/Texture";
    private const string KMainTex = "_MainTex";
    private static readonly int MainTex = Shader.PropertyToID(KMainTex);
    
    [Header("Panel Configuration")]
    [Tooltip("Width of the panel in pixels.")] 
    [SerializeField] private int panelWidth = 1280;

    [Tooltip("Height of the panel in pixels.")] 
    [SerializeField] private int panelHeight = 720;

    [Tooltip("Scale of the panel (like zoom in a browser).")] 
    [SerializeField] private float panelScale = 1.0f;

    [Tooltip("Pixels per world unit. Determines the real-world size of the panel.")] 
    [SerializeField] private float pixelsPerUnit = 500.0f;

    [Header("Dependencies")]
    [Tooltip("Visual tree asset for this panel.")] 
    [SerializeField] private VisualTreeAsset visualTreeAsset;

    [Tooltip("PanelSettings prefab instance.")] 
    [SerializeField] private PanelSettings panelSettingsAsset;

    [Tooltip("RenderTexture prefab instance.")] 
    [SerializeField] private RenderTexture renderTextureAsset;
    
    private MeshRenderer _meshRenderer;
    private UIDocument _uiDocument;
    private PanelSettings _panelSettings;
    private RenderTexture _renderTexture;
    private Material _material;

    #endregion

    private void Awake()
    {
        InitializeComponents();
        BuildPanel();
    }

    public void SetLabelText(string label, string text)
    {
        if (_uiDocument.rootVisualElement == null) {
            _uiDocument.visualTreeAsset = visualTreeAsset;
        }
        
        // Consider caching the label element for better performance
        _uiDocument.rootVisualElement.Q<Label>(label).text = text;
    }

    private void InitializeComponents()
    {
        InitializeMeshRenderer();
        
        // Optionally add a box collider to the object
        // BoxCollider boxCollider = gameObject.GetOrAdd<BoxCollider>();
        // boxCollider.size = new Vector3(1, 1, 0);
        
        var meshFilter = GetOrAdd<MeshFilter>();
        meshFilter.sharedMesh = GetQuadMesh();
    }

    private T GetOrAdd<T>() where T : Component
    {
        var result = GetComponent<T>();
        if (result == null)
        {
            result = gameObject.AddComponent<T>();
        }

        return result;
    }
    
    
    private void InitializeMeshRenderer()
    {
        _meshRenderer = GetOrAdd<MeshRenderer>();
        _meshRenderer.sharedMaterial = null;
        _meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;
        _meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
        _meshRenderer.lightProbeUsage = LightProbeUsage.Off;
        _meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    }

    private void BuildPanel()
    {
        CreateRenderTexture();
        CreatePanelSettings();
        CreateUIDocument();
        CreateMaterial();
        
        SetMaterialToRenderer();
        SetPanelSize();
    }

    private void CreateRenderTexture()
    {
        RenderTextureDescriptor descriptor = renderTextureAsset.descriptor;
        descriptor.width = panelWidth;
        descriptor.height = panelHeight;
        _renderTexture = new RenderTexture(descriptor) {
            name = $"{name} - RenderTexture"
        };
    }

    private void CreatePanelSettings()
    {
        _panelSettings = Instantiate(panelSettingsAsset);
        _panelSettings.targetTexture = _renderTexture;
        _panelSettings.clearColor = true;
        _panelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
        _panelSettings.scale = panelScale;
        _panelSettings.name = $"{name} - PanelSettings";
    }

    private void CreateUIDocument()
    {
        _uiDocument = GetOrAdd<UIDocument>();
        _uiDocument.panelSettings = _panelSettings;
        _uiDocument.visualTreeAsset = visualTreeAsset;
    }

    private void CreateMaterial()
    {
        var shaderName = _panelSettings.colorClearValue.a < 1.0f ? KTransparentShader : KTextureShader;
        _material = new Material(Shader.Find(shaderName));
        _material.SetTexture(MainTex, _renderTexture);
    }

    private void SetMaterialToRenderer()
    {
        if (_meshRenderer != null) {
            _meshRenderer.sharedMaterial = _material;
        }
    }

    private void SetPanelSize()
    {
        if (_renderTexture != null && (_renderTexture.width != panelWidth || _renderTexture.height != panelHeight)) {
            _renderTexture.Release();
            _renderTexture.width = panelWidth;
            _renderTexture.height = panelHeight;
            _renderTexture.Create();
            
            _uiDocument.rootVisualElement?.MarkDirtyRepaint();
        }
        
        transform.localScale = new Vector3(panelWidth / pixelsPerUnit, panelHeight / pixelsPerUnit, 1.0f);
    }

    private static Mesh GetQuadMesh()
    {
        var tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        var quadMesh = tempQuad.GetComponent<MeshFilter>().sharedMesh;
        Destroy(tempQuad);
        
        return quadMesh;
    }
    
    private void DestroyGeneratedAssets ()
    {
        if (_uiDocument) Destroy(_uiDocument);
        if (_renderTexture) Destroy(_renderTexture);
        if (_panelSettings) Destroy(_panelSettings);
        if (_material) Destroy(_material);
    }

    private void OnDestroy ()
    {
        DestroyGeneratedAssets();
    }   
}