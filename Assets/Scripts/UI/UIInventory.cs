using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventory : MonoBehaviour
{
    [SerializeField] PlayerInventory _inventory;
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _referenceItemObject;

    List<GameObject> _itemsDisplayed = new();

    private void Start()
    {
        if (_inventory == null) _inventory = FindObjectOfType<PlayerInventory>();
        _referenceItemObject.SetActive(false);
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
        GameObject newItem = GameObject.Instantiate(_referenceItemObject);
        newItem.name = item.Id.ToString();
        newItem.transform.SetParent(_panel.transform, false);
        newItem.GetComponent<Image>().sprite = item.Icon;
        newItem.SetActive(true);
        _itemsDisplayed.Add(newItem);

        if (item.Amount > 1)
        {
            GameObject counter = newItem.transform.GetChild(0).gameObject;
            counter.GetComponent<TextMeshProUGUI>().text = item.Amount.ToString();
            counter.SetActive(true);
        }
    }
}
