using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _itemPrefab;

    List<GameObject> _itemsDisplayed = new();

    public void Start()
    {
        if (_inventory == null) _inventory = FindObjectOfType<PlayerInventory>();
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        foreach (GameObject item in _itemsDisplayed) Destroy(item);
        _itemsDisplayed = new List<GameObject>();

        foreach (InventoryItem item in _inventory.GetAllItems())
        {
            if (item.Amount == 0) continue;
            AddToPanel(item);
        }
    }

    private void AddToPanel(InventoryItem item)
    {
        GameObject newItem = GameObject.Instantiate(_itemPrefab);
        newItem.name = item.Id.ToString();
        newItem.transform.SetParent(_panel.transform, false);
        newItem.GetComponent<Image>().sprite = item.Icon;
        _itemsDisplayed.Add(newItem);

        if (item.Amount > 1)
        {
            GameObject counter = newItem.transform.GetChild(0).gameObject;
            counter.GetComponent<TextMeshProUGUI>().text = item.Amount.ToString();
            counter.SetActive(true);
        }
    }
}
