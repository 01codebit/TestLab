using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Channels;
using Logging;
using TestLab.EventChannel.Model;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TestLab.EventChannel.View
{
    public class ListItemPresenterSOAP : MonoBehaviour
    {
        // vista
        // [SerializeField] private PoolManager _poolManager;
        [SerializeField] private NetworkEventChannelSO _networkEventChannel;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _anchor;
        [SerializeField] private TextMeshProUGUI _countLabel;

        [SerializeField] private TodoListSO _todoListSO;

        private GameObjectPool _gameObjectPool;
        private long _lastLoadingTime;
        
        private static string _endpoint = "todos";
        private static Dictionary<string, string> _parameters = new Dictionary<string, string>()
        {
            {"userId","1"}, 
            {"completed","false"}
        };

        private void OnEnable()
        {
            _networkEventChannel.OnEventRaisedAsync += LoadData;

            if (_todoListSO != null)
            {
                _todoListSO.OnDataChange += HandleReload;
                _todoListSO.OnDataClear += HandleClear;
            }
        }

        private void OnDisable()
        {
            _networkEventChannel.OnEventRaisedAsync -= LoadData;

            if (_todoListSO != null)
            {
                _todoListSO.OnDataChange -= HandleReload;
                _todoListSO.OnDataClear -= HandleClear;
                
                _todoListSO.ClearItems();
            }
        }

        private void Start()
        {
            _gameObjectPool = new GameObjectPool(_itemPrefab);
            // LoadData();
        }


        private List<TodoDataSimpleView> _itemViews = new List<TodoDataSimpleView>();
        
        private void HandleReload()
        {
            ConditionalLogger.Log($"[ListItemPresenter.HandleReload] (before) {_gameObjectPool.Stats()}");
            
            HandleClear();
            //_gameObjectPool.Clear();

            if (_todoListSO == null) return;

            List<Todo> data = _todoListSO.Items;


            foreach (var todo in data)
            {
                var item = _gameObjectPool.GetAnchoredObject(_anchor);
                item.name = todo.userId + "_" + todo.id;
                var dataView = item.GetComponent<TodoDataSimpleView>();
                dataView.Bind(todo, _gameObjectPool.GetPool());
                _itemViews.Add(dataView);
            }

            ConditionalLogger.Log($"[ListItemPresenter.HandleReload] (after) {_gameObjectPool.Stats()}");
            _countLabel.text = $"Count: {data.Count} ({_lastLoadingTime}ms)";
        }
        
        private async Task LoadData()
        {
            var sw = new Stopwatch();
            sw.Start();
            ConditionalLogger.Log("[ListItemPresenter.LoadData] start");
            var todos = await HttpService.GetFromJsonAsync<Todo>(_endpoint, _parameters);

            if(_todoListSO!=null)
            {
                _todoListSO.SetItems(todos);
            }

            sw.Stop();
            _lastLoadingTime = sw.ElapsedMilliseconds;
            // ConditionalLogger.Log($"[ListItemPresenter.LoadData] end ({_lastLoadingTime}ms) result: {result}");
        }

        // potrebbe essere utile per non dare agli item un riferimento al pool
        public void ReleaseItem(GameObject go)
        {
            _gameObjectPool.ReleaseObject(go);
        }

        private void HandleClear()
        {
            for (int i = _itemViews.Count; i > 0; i--)
            {
                _itemViews[i - 1].Dispose();
            }
            _itemViews.Clear();
        }
    }
}