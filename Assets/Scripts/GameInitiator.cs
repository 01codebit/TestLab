using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private bool _disableNotMainCamerasAndLights;
    [SerializeField] private List<GameElement> _elements = new();
    [SerializeField] private List<string> _additiveScenes = new();

    private Dictionary<string, GameObject> _sceneElements = new();

    private async Task Start()
    {
        await InitializeGame();
    }

    private async Task InitializeGame()
    {
        Debug.Log($"[GameInitiator.InitializeGame] ------------------ [begin]");
        AddElements();

        await AddScenes();

        if (_disableNotMainCamerasAndLights)
        {
            Debug.Log($"[GameInitiator.InitializeGame] disable not main camera and lights");
            var dis = DisableCamerasAndLights();
            Debug.Log($"[GameInitiator.InitializeGame] disabled {dis.Item1} cameras and {dis.Item2} lights");
        }
        
        Debug.Log($"[GameInitiator.InitializeGame] ------------------ [end]");
    }

    private void AddElements()
    {
        var el = _elements.OrderBy(x => x.Priority);
        var elementCount = el.Count();
        var currentElement = 1;
        foreach (var x in el)
        {
            Debug.Log($"[GameInitiator.AddElements] instantiate element '{x.Description}' [{currentElement}/{elementCount}]");
            var inst = Instantiate(x.Prefab);

            if (!string.IsNullOrEmpty(x.Description))
            {
                inst.gameObject.name = x.Description;
                _sceneElements.Add(x.Description, inst);
            }
            else
            {
                var n = inst.gameObject.name;
                n = n.Replace("(Clone)", "");
                inst.gameObject.name = n;
            }

            currentElement++;
        }
    }

    private async Task AddScenes()
    {
        var sceneCount = _additiveScenes.Count;
        var currentScene = 1;
        foreach (var s in _additiveScenes)
        {
            Debug.Log($"[GameInitiator.AddScenes] load scene additive '{s}' [{currentScene}/{sceneCount}]");
            await SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);
            currentScene++;
        }
    }

    private Tuple<int, int> DisableCamerasAndLights()
    {
        var disabledCams = 0;
        foreach (var cam in Camera.allCameras)
        {
            if (cam.tag.Equals("MainCamera")) continue;
            cam.enabled = false;
            disabledCams++;
        }

        var disabledLights = 0;
        var lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (var l in lights)
        {
            if (l.tag.Equals("MainLight")) continue;
            l.enabled = false;
            disabledLights++;
        }

        return new Tuple<int, int>(disabledCams, disabledLights);
    }
}