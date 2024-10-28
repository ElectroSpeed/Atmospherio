using UnityEngine;

public class ItemInitializer : MonoBehaviour
{
    public Item[] _items;
    public int _number;
    private Inventory _inventory;

    private void Start()
    {
        _inventory = FindObjectOfType<Inventory>();

        foreach (var item in _items)
        {
            _inventory.AddItem(item, _number);
        }
    }
}
