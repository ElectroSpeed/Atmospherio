using System.Collections;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    private Inventory _inventory;

    private void Start()
    {
        _inventory = this.GetComponent<Inventory>();
    }

    public void CraftNewItem(CraftReciepe receipe)
    {
        foreach (ComponentCraft item in receipe._componentCraftList)
        {
            if (!_inventory.CheckNumberItem(item._itemComponentCraft, item._quantityComponentCraft))
            {
                Debug.Log("Pas assez de composants pour craft.");
                return;
            }
        }

        if (!CanAddCraftedItem(receipe))
        {
            Debug.Log("Inventaire plein, craft impossible.");
            return;
        }

        foreach (ComponentCraft item in receipe._componentCraftList)
        {
            _inventory.RemoveItem(item._itemComponentCraft, item._quantityComponentCraft);
        }

        _inventory.AddItem(receipe._itemCraft, receipe._itemCraftCount);
    }

    private bool CanAddCraftedItem(CraftReciepe receipe)
    {
        foreach (InventorySlot slot in _inventory._slots)
        {
            if (slot._item != null && slot._item._itemName == receipe._itemCraft._itemName && !slot.IsFull())
            {
                return true;
            }
        }

        foreach (InventorySlot slot in _inventory._slots)
        {
            if (slot.IsEmpty())
            {
                return true;
            }
        }

        int slotsLibres = 0;
        foreach (ComponentCraft item in receipe._componentCraftList)
        {
            foreach (InventorySlot slot in _inventory._slots)
            {
                if (slot._item != null && slot._item._itemName == item._itemComponentCraft._itemName && slot._quantity <= item._quantityComponentCraft)
                {
                    slotsLibres++;
                    break;
                }
            }
        }

        return slotsLibres > 0;
    }
}
