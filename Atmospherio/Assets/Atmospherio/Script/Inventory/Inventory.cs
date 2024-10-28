using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> _slots = new List<InventorySlot>();
    public int _maxSlots = 20;

    public bool AddItem(Item item, int quantity)
    {
        foreach (var slot in _slots)
        {
            if (slot._item == item && !slot.IsFull())
            {
                int spaceLeft = item._maxStack - slot._quantity;
                if (quantity <= spaceLeft)
                {
                    slot._quantity += quantity;
                    return true;
                }
                else
                {
                    slot._quantity = item._maxStack;
                    quantity -= spaceLeft;
                }
            }
        }

        while (quantity > 0 && _slots.Count < _maxSlots)
        {
            int amountToAdd = Mathf.Min(quantity, item._maxStack);
            _slots.Add(new InventorySlot(item, amountToAdd));
            quantity -= amountToAdd;
        }

        return quantity == 0;
    }

    public void RemoveItem(Item item, int quantity)
    {
        for (int i = _slots.Count - 1; i >= 0 && quantity > 0; i--)
        {
            if (_slots[i]._item == item)
            {
                if (_slots[i]._quantity <= quantity)
                {
                    quantity -= _slots[i]._quantity;
                    _slots.RemoveAt(i);
                }
                else
                {
                    _slots[i]._quantity -= quantity;
                    quantity = 0;
                }
            }
        }
    }
}
