using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private VariableStorageBehaviour _variableStorage;

    [System.Serializable]
    private class Events
    {
        public UnityEvent ItemAdded = new UnityEvent();
        public UnityEvent ItemRemoved = new UnityEvent();
    }
    [SerializeField] private AudioSource _UISFXAudioSource;
    [SerializeField] private AudioClip _addItemClip;
    [SerializeField] private AudioClip _removeItemClip;
    [SerializeField] private Events _events;

    [SerializeField] private List<InventoryItem> _items;

    public bool ContainsItem(int id)
    {
        if (_items.Find(x => x.Id == id) != null) return true;
        else return false;
    }

    public int ContainsItemAmount(int id, int amount)
    {
        InventoryItem item = _items.Find(x => x.Id == id);
        if (item != null) return item.Amount;
        else return 0;
    }

    public InventoryItem GetItem(int id)
    {
        return _items.Find(x => x.Id == id);
    }

    public InventoryItem[] GetAllItems()
    {
        return _items.ToArray();
    }

    [YarnCommand("add_item")]
    public bool AddItem(int id, int amount = 1)
    {
        InventoryItem item = _items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount += amount;
            SetInVariableStorage(id, item.Amount);
            _events.ItemAdded.Invoke();
            _UISFXAudioSource.PlayOneShot(_addItemClip);
            return true;
        }
        else return false;
    }

    [YarnCommand("remove_item")]
    public bool RemoveItem(int id, int amount = 1)
    {
        InventoryItem item = _items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount = Mathf.Max(item.Amount - amount, 0);
            SetInVariableStorage(id, item.Amount);
            _events.ItemRemoved.Invoke();
            _UISFXAudioSource.PlayOneShot(_removeItemClip);
            return true;
        }
        else return false;
    }

    [YarnCommand("update_inventory")]
    private void SyncVariableStorage()
    {
        (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) allVariables = _variableStorage.GetAllVariables();
        foreach (string key in allVariables.Item1.Keys)
        {
            if (!key.StartsWith("$item_")) continue;

            int id = Int32.Parse(key.Split("_")[1]);
            int amount = Mathf.FloorToInt(allVariables.Item1[key]);
            InventoryItem itemInInventory = _items.Find(x => x.Id == id);

            if (itemInInventory.Amount == amount) continue;

            bool add = amount > itemInInventory.Amount ? true : false;
            itemInInventory.Amount = amount;
            if (add) _events.ItemAdded.Invoke();
            else _events.ItemRemoved.Invoke();
        }
    }

    private void SetInVariableStorage(int id, int totalAmount)
    {
        string variableName = "$item_" + id.ToString();
        _variableStorage.SetValue(variableName, totalAmount);
    }
}
