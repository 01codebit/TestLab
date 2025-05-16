using System;
using UnityEngine;

[Serializable]
public class GameElement
{
    [SerializeField] private int _priority = 1;
    
    [SerializeField] private string _description = String.Empty;

    [SerializeField] private GameObject _prefab = null;

    public GameElement()
    {
        _description = String.Empty;
        _prefab = null;
    }
    
    public int Priority {
        get => _priority;
        private set => _priority = value;
    }

    public string Description {
        get => _description;
        private set => _description = value;
    }
    
    public GameObject Prefab {
        get => _prefab;
        private set => _prefab = value;
    }
}
