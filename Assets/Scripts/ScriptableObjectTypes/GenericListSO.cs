using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericListSO", menuName = "Scriptable Objects/GenericListSO")]
public abstract class GenericListSO<T> : ScriptableObject
{
    public event Action OnDataChange;
    public event Action OnDataClear;

    public List<T> Items = new List<T>();

    public void SetItems(List<T> value)
    {
        if (value == null)
        {
            value = new List<T>();
        }

        Items = value;
        OnDataChange?.Invoke();
    }

    public void AddItem(T item, bool notify = false)
    {
        if (Items == null)
        {
            Items = new List<T>();
        }
        Items.Add(item);

        if (notify)
        {
            OnDataChange?.Invoke();
        }
    }

    public void RemoveItem(T item, bool notify = false)
    {
        if (Items != null && Items.Contains(item))
        {
            Items.Remove(item);

            if (notify)
            {
                OnDataChange?.Invoke();
            }
        }
    }

    public void ClearItems(bool notify = false)
    {
        if (Items != null)
        {
            Items.Clear();

            if (notify)
            {
                OnDataClear?.Invoke();
            }
        }
    }

    public void NotifyChange()
    {
        OnDataChange?.Invoke();
    }

    public void NotifyClear()
    {
        OnDataClear?.Invoke();
    }
}