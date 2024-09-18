using System;
using TestLab.EventChannel.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace TestLab.EventChannel.View
{
    public class TodoDataView : DataView
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _description;

        private IEventBasedData _model;
        private IObjectPool<GameObject> _pool;
        private int _titleMaxLen = 12;
        
        public override void Bind(IDataModel data, IObjectPool<GameObject> pool, IEventBasedData model)
        {
            _title.text = data.GetField("id");
            var title = data.GetField("title");
            _description.text = title.Length > _titleMaxLen ? 
                _description.text = title.Substring(0, _titleMaxLen) + "..." :
                _description.text = title;

            _pool = pool;

            _model = model;
            _model.OnDataClear += Dispose;
        }
        
        private void Dispose()
        {
            _model.OnDataClear -= Dispose;
            _pool.Release(gameObject);
        }
    }
}