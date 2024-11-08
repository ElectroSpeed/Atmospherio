using System.Collections;
using UnityEngine;

public class ItemInitializer : MonoBehaviour
{
    public Item[] _item;
    public int _number;
    private Inventory _inventory;

    public void InitializeItem()
    {
        _inventory = FindFirstObjectByType<Inventory>();
        if (_inventory == null)
        {
            return;
        }
        foreach(var item in _item)
        {
            _inventory.AddItem(item, _number);
        }
        StartCoroutine(RemoveItemInInventory());
    }

    public IEnumerator RemoveItemInInventory()
    {
        yield return new WaitForSeconds(5f);
        foreach (var item in _item)
        {
            _inventory.RemoveItem(item, 0);
        }
    }
}
