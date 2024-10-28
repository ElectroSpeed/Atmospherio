using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item _item;
    public int _quantity;

    public InventorySlot(Item item, int quantity)
    {
        this._item = item;
        this._quantity = quantity;
    }

    public bool IsFull()
    {
        return _quantity >= _item._maxStack;
    }
}
