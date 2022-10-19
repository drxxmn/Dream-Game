using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    private class Events
    {
        public UnityEvent ItemAdded = new UnityEvent();
        public UnityEvent ItemRemoved = new UnityEvent();
    }
    [SerializeField] private Events _events;
    [SerializeField] private List<InventoryItem> items;

    public bool ContainsItem(int id)
    {
        if (items.Find(x => x.Id == id) != null) return true;
        else return false;
    }

    public int ContainsItemAmount(int id, int amount)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null) return item.Amount;
        else return 0;
    }

    public InventoryItem GetItem(int id)
    {
        return items.Find(x => x.Id == id);
    }

    [YarnCommand("add_item")]
    public bool AddItem(int id, int amount = 1)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount += amount;
            _events.ItemAdded.Invoke();
            return true;
        }
        else return false;
    }

    [YarnCommand("remove_item")]
    public bool RemoveItem(int id, int amount = 1)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount = Mathf.Max(item.Amount - amount, 0);
            _events.ItemRemoved.Invoke();
            return true;
        }
        else return false;
    }
}
