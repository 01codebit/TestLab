using TestLab.EventChannel.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace TestLab.EventChannel.View
{
    public class TodoDataSimpleView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        private IDataModel _model;
        private IObjectPool<GameObject> _pool;
        private int _titleMaxLen = 12;
        
        public void Bind(Todo data, IObjectPool<GameObject> pool)
        {
            _pool = pool;
            _model = data;

            _title.text = _model.GetField("id");
            var title = _model.GetField("title");
            
            _description.text = title.Length > _titleMaxLen ? 
                _description.text = title.Substring(0, _titleMaxLen) + "..." :
                _description.text = title;

        }
        
        public void Dispose()
        {
            _title.text = "id";
            _description.text = "description"; 
            gameObject.name = "TodoItemSimple";
            
            _pool.Release(gameObject);
        }
    }
}