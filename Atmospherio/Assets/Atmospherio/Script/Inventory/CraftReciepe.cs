using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class CraftReciepe : MonoBehaviour
{
    public Item _itemCraft;
    public List<ComponentCraft> _componentCraftList = new();
    public List<TMP_Text> _componentCraftText = new();
    public int _itemCraftCount;

    private Inventory _inventory;

    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
    }

    private void Update()
    {
        VisualUpdate();
    }

    private void VisualUpdate()
    {
        foreach (TMP_Text text in _componentCraftText)
        {
            ComponentCraft component = _componentCraftList[_componentCraftText.IndexOf(text)];
            text.text = component._quantityComponentCraft.ToString();
            if (!_inventory.CheckNumberItem(component._itemComponentCraft, component._quantityComponentCraft))
            {
                text.color = Color.red;
            }
            else
            {
                text.color = Color.green;
            }
        }
    }
}
