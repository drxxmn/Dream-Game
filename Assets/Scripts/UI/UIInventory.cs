using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _referenceItemObject;

    List<InventoryItem> _itemsDisplayed = new();

    private void Start()
    {
        if (_inventory == null) _inventory = FindObjectOfType<PlayerInventory>();
        _referenceItemObject.SetActive(false);
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        foreach (InventoryItem item in _inventory.GetAllItems())
        {
            if (item.Amount == 0)
            {
                if (_itemsDisplayed.Contains(item))
                {
                    RemoveFromPanel(item);
                }
            }
            else AddToPanel(item);
        }
    }

    private void AddToPanel(InventoryItem item)
    {
        GameObject newItem = GameObject.Instantiate(_referenceItemObject);
        newItem.name = item.Id.ToString();
        newItem.transform.SetParent(_panel.transform, false);
        newItem.GetComponent<Image>().sprite = item.Icon;
        newItem.SetActive(true);
        _itemsDisplayed.Add(item);
    }

    private void RemoveFromPanel(InventoryItem item)
    {
        GameObject toRemove = _panel.transform.Find(item.Id.ToString()).gameObject;
        Destroy(toRemove);
        _itemsDisplayed.Remove(item);
    }
}
