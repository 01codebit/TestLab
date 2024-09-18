using System.Collections.Generic;
using System.Diagnostics;
using Logging;
using TestLab.EventChannel.Model;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TestLab.EventChannel.View
{
    public class ListItemPresenter : MonoBehaviour
    {
        // vista
        // [SerializeField] private PoolManager _poolManager;
        [SerializeField] private NetworkEventChannelSO _networkEventChannel;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _anchor;
        [SerializeField] private TextMeshProUGUI _countLabel;

        private GameObjectPool _gameObjectPool;
        private long _lastLoadingTime;
        
        // modello
        private DataStructure<Todo> _model = new DataStructure<Todo>();
        
        private static string _endpoint = "todos";
        private static Dictionary<string, string> _parameters = new Dictionary<string, string>()
        {
            {"userId","1"}, 
            {"completed","false"}
        };

        private void OnEnable()
        {
            _model.OnDataReloaded += HandleReload;
            _networkEventChannel.OnEventRaised += LoadData;
        }

        private void OnDisable()
        {
            _model.OnDataReloaded -= HandleReload;
            _networkEventChannel.OnEventRaised -= LoadData;
        }

        private void Start()
        {
            _gameObjectPool = new GameObjectPool(_itemPrefab);
            // LoadData();
        }

        private void HandleReload()
        {
            // ConditionalLogger.Log("[ListItemPresenter.HandleReload]");
            ConditionalLogger.Log($"[ListItemPresenter.HandleReload] (before) {_gameObjectPool.Stats()}");

            // foreach (var cc in _anchor.GetComponentsInChildren<TodoDataView>())
            // {
            //     cc.Dispose();
            // }
            _gameObjectPool.Clear();
            
            var data = _model.Data;
            // _poolManager?.SetItemList(data);

            foreach (var todo in data)
            {
                var item = _gameObjectPool.GetAnchoredObject(_anchor);
                item.name = todo.userId + "_" + todo.id;
                var dataView = item.GetComponent<DataView>();
                dataView.Bind(todo, _gameObjectPool.GetPool(), _model);
            }
            ConditionalLogger.Log($"[ListItemPresenter.HandleReload] (after) {_gameObjectPool.Stats()}");
            _countLabel.text = $"Count: {data.Count} ({_lastLoadingTime}ms)";
        }
        
        private async void LoadData()
        {
            _model.ResetData();
            // _poolManager.Clear();
            
            var sw = new Stopwatch();
            sw.Start();
            // ConditionalLogger.Log("[ListItemPresenter.LoadData] start");
            await HttpService.GetFromJsonAsync<Todo>(_endpoint, _parameters, _model);
            sw.Stop();
            _lastLoadingTime = sw.ElapsedMilliseconds;
            // ConditionalLogger.Log($"[ListItemPresenter.LoadData] end ({_lastLoadingTime}ms) result: {result}");
        }

        // potrebbe essere utile per non dare agli item un riferimento al pool
        public void ReleaseItem(GameObject go)
        {
            _gameObjectPool.ReleaseObject(go);
        }
    }
}