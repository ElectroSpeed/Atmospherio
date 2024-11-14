using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject _emptyItem;
    public List<InventorySlot> _slots = new List<InventorySlot>();

    public bool AddItemInChest(Item item, int quantity, GameObject chest)
    {
        List<InventorySlot> chestSlots = new List<InventorySlot>();
        for(int i = 0; i < chest.transform.childCount; i++)
        {
            if (chest.transform.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                chestSlots.Add(chest.transform.GetChild(i).GetComponent<InventorySlot>());
            }
        }

        if (chestSlots.Count == 0)
        {
            Debug.LogError("Pas de slots d'inventaire disponibles.");
            return false;
        }

        List<InventorySlot> slotsEmpty = new List<InventorySlot>();
        List<InventorySlot> slotsSeemsItem = new List<InventorySlot>();

        foreach (InventorySlot slot in chestSlots)
        {
            if (slot._item != null && slot._item._itemName == item._itemName && !slot.IsFull())
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
                return true;
            }
        }
        foreach (InventorySlot slot in slotsEmpty)
        {
            GameObject childItem = Instantiate(_emptyItem, slot.transform);
            int toAdd = Mathf.Min(quantity, item._maxStack);
            slot._item = item;
            slot._quantity = toAdd;
            quantity -= toAdd;
            childItem.GetComponent<ItemUI>().SetItem(slot);

            if (quantity <= 0)
            {
                return true;
            }
        }
        return false;
    }

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
            if (slot._item != null && slot._item._itemName == item._itemName && !slot.IsFull())
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
                return;
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
                return;
            }
        }
    }
    
    public bool CheckNumberItem(Item item, int requiredCount)
    {
        int itemCount = 0;
        foreach (InventorySlot slot in _slots)
        {
            if (slot._item != null && slot._item._itemName == item._itemName)
            {
                itemCount += slot._quantity;
            }
        }
        if (itemCount < requiredCount)
        {
            return false;
        }
        else
        {
            return true;
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
    public GameObject GetEmptyItem() => _emptyItem;
}
