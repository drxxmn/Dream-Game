using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> items;

    public bool InventoryContainsItem(int id)
    {
        if (items.Find(x => x.Id == id) != null) return true;
        else return false;
    }

    public int InventoryContainsItemAmount(int id, int amount)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null) return item.Amount;
        else return 0;
    }

    public bool AddItemToInventory(int id, int amount = 1)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount += amount;
            return true;
        }
        else return false;
    }

    public bool RemoveItemFromInventory(int id, int amount = 1)
    {
        InventoryItem item = items.Find(x => x.Id == id);
        if (item != null)
        {
            item.Amount = Mathf.Max(item.Amount - amount, 0);
            return true;
        }
        else return false;
    }
}
