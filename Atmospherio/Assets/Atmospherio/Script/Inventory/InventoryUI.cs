using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Settings")]

    [SerializeField] GameObject _inventorySlotPrefab;
    [SerializeField] Transform _inventoryPanelVisible;
    [SerializeField] Transform _inventoryPanelInvisible;

    public int _totalSlots;
    public int _slotsPerLine;
    public int _slotInVisiblePanel;
    public bool _inventoryOpen = false;

    public Vector2 _slotSize = new Vector2(100, 100);
    public float _slotSpacing = 0f;

    private Inventory _inventory;
    private ItemInitializer _itemInitializer;

    private void Start()
    {
        _itemInitializer = FindFirstObjectByType<ItemInitializer>();
        _inventory = FindFirstObjectByType<Inventory>();
        GenerateInventoryGrid();
        _itemInitializer.InitializeItem();
    }

    private void GenerateInventoryGrid()
    {
        int lineCount = Mathf.CeilToInt((float)_totalSlots / _slotsPerLine);

        float gridWidth = (_slotsPerLine * _slotSize.x) + ((_slotsPerLine - 1) * _slotSpacing);
        float gridHeight = (lineCount * _slotSize.y) + ((lineCount - 1) * _slotSpacing);

        Vector2 startPosition = new Vector2(-gridWidth / 2 + _slotSize.x / 2, gridHeight / 2 - _slotSize.y / 2);

        for (int i = 0; i < _totalSlots; i++)
        {
            int line = i / _slotsPerLine;
            int column = i % _slotsPerLine;

            Vector2 slotPosition = startPosition + new Vector2(column * (_slotSize.x + _slotSpacing), -line * (_slotSize.y + _slotSpacing));

            GameObject slot;
            if (i >= _slotInVisiblePanel)
            {
                slot = Instantiate(_inventorySlotPrefab, _inventoryPanelInvisible);
                _inventory._slots.Add(slot.GetComponent<InventorySlot>());
            }
            else
            {
                slot = Instantiate(_inventorySlotPrefab, _inventoryPanelVisible);
                _inventory._slots.Add(slot.GetComponent<InventorySlot>());
            }
            slot.GetComponent<RectTransform>().anchoredPosition = slotPosition;
        }
    }
    public void OpenInventory()
    {
        if (!_inventoryOpen)
        {
            _inventoryPanelInvisible.gameObject.SetActive(true);
            _inventoryOpen = true;
        }
        else
        {
            _inventoryPanelInvisible.gameObject.SetActive(false);
            _inventoryOpen = false;
        }
    }
}
