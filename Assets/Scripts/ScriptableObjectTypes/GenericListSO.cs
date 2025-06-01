using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenericListSO", menuName = "Scriptable Objects/GenericListSO")]
public abstract class GenericListSO<T> : ScriptableObject
{
    public List<T> Items;

    public void AddItem(T item)
    {
        if (Items == null)
        {
            Items = new List<T>();
        }
        Items.Add(item);
    }

    public void RemoveItem(T item)
    {
        if (Items != null && Items.Contains(item))
        {
            Items.Remove(item);
        }
    }

    public void ClearItems()
    {
        if (Items != null)
        {
            Items.Clear();
        }
    }
}
