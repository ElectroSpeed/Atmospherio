using UnityEngine;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    public Item _item;
    public int _quantity;

    public void ResetSlot()
    {
        _item = null;
        _quantity = 0;
    }

    public void SetSlot(ItemUI itemUI)
    {
        _item = itemUI._item;
        _quantity = itemUI._count;
        itemUI.SetItem(this);
    }

    public bool IsFull()
    {
        return _quantity >= _item._maxStack;
    }

    public bool IsEmpty()
    {
        return _item == null || _quantity <= 0;
    }
}
