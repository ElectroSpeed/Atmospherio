using System.Linq;
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
        if (receipe._componentCraftList.Any(item => !_inventory.CheckNumberItem(item._itemComponentCraft, item._quantityComponentCraft)))
        {
            Debug.Log("Pas assez de composants pour craft.");
            return;
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

    public void UpgradeOxyBulle(CraftReciepe receipe)
    {
        if (receipe._componentCraftList.Any(item => !_inventory.CheckNumberItem(item._itemComponentCraft, item._quantityComponentCraft)))
        {
            Debug.Log("Pas assez de composants pour Upgrade.");
            return;
        }
        foreach (ComponentCraft item in receipe._componentCraftList)
        {
            _inventory.RemoveItem(item._itemComponentCraft, item._quantityComponentCraft);
        }

        ZoneManager bubbleZone = FindFirstObjectByType<ZoneManager>();
        bubbleZone.UpgradeZone(1);
    }

    public bool CanMake(CraftReciepe receipe)
    {
        return receipe._componentCraftList.All(item => _inventory.CheckNumberItem(item._itemComponentCraft, item._quantityComponentCraft));
    }

    private bool CanAddCraftedItem(CraftReciepe receipe)
    {
        if (_inventory._slots.Any(slot => slot._item != null && slot._item._itemName == receipe._itemCraft._itemName && !slot.IsFull()))
        {
            return true;
        }

        if (_inventory._slots.Any(slot => slot.IsEmpty()))
        {
            return true;
        }

        int slotsLibre = receipe._componentCraftList.Count(item => _inventory._slots.Any(slot => slot._item != null && slot._item._itemName == item._itemComponentCraft._itemName && slot._quantity <= item._quantityComponentCraft));
        return slotsLibre > 0;
    }
}
