using NUnit.Framework;
using UnityEngine;

public class CraftItem : MonoBehaviour
{
    private Inventory _inventory;

    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
    }

    public void CraftNewItem(Item itemCraft, ComponentCraft[] listComponentForCraft)
    {
        foreach (ComponentCraft item in listComponentForCraft)
        {
        }
    }
}
