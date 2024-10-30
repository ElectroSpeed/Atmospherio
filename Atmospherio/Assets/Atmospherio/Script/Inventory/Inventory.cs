using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _emptyItem;
    public List<InventorySlot> _slots = new List<InventorySlot>();
    private InventoryUI _inventoryUI;

    public void AddItem(Item item, int quantity)
    {
        if (_slots.Count == 0)
        {
            Debug.LogError("Pas de slots d'inventaire disponibles.");
            return;
        }

        List<InventorySlot> slotsEmpty = new List<InventorySlot>();
        List<InventorySlot> slotsSeemsItem = new List<InventorySlot>();

        foreach (InventorySlot slot in _slots)
        {
            if (slot._item == item && !slot.IsFull())
            {
                slotsSeemsItem.Add(slot);
            }
            else if (slot.IsEmpty())
            {
                slotsEmpty.Add(slot);
            }
        }
        foreach (InventorySlot slot in slotsSeemsItem)
        {
            ItemUI childItem = slot.transform.GetChild(0).GetComponent<ItemUI>();
            int spaceInSlot = item._maxStack - slot._quantity;
            int toAdd = Mathf.Min(quantity, spaceInSlot);
            slot._quantity += toAdd;
            quantity -= toAdd;
            childItem.SetItem(slot);

            if (quantity <= 0)
            {
                break;
            }
        }
        foreach(InventorySlot slot in slotsEmpty)
        {
            GameObject childItem = Instantiate(_emptyItem, slot.transform);
            int toAdd = Mathf.Min(quantity, item._maxStack);
            slot._item = item;
            slot._quantity = toAdd;
            quantity -= toAdd;
            childItem.GetComponent<ItemUI>().SetItem(slot);

            if (quantity <= 0)
            {
                break;
            }
        }
    }

    public void RemoveItem(Item item, int quantity)
    {
        for (int i = _slots.Count - 1; i >= 0 && quantity > 0; i--)
        {
            if (_slots[i]._item == item)
            {
                ItemUI childItem = _slots[i].transform.GetChild(0).GetComponent<ItemUI>();
                if (_slots[i]._quantity <= quantity)
                {
                    quantity -= _slots[i]._quantity;
                    _slots[i].ResetSlot();
                    Destroy(childItem.gameObject);
                }
                else
                {
                    _slots[i]._quantity -= quantity;
                    childItem.SetItem(_slots[i]);
                    quantity = 0;
                }
            }
        }
    }
}
