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
                return;
            }
        }
        foreach (ComponentCraft item in receipe._componentCraftList)
        {
            _inventory.RemoveItem(item._itemComponentCraft, item._quantityComponentCraft);
        }
        _inventory.AddItem(receipe._itemCraft, receipe._itemCraftCount);
    }
}
