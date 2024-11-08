using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Settings")]

    [SerializeField] GameObject _inventorySlotPrefab;
    [SerializeField] Transform _inventoryPanelVisible;
    [SerializeField] Transform _inventoryPanelInvisible;
    [SerializeField] GameObject _inventoryTopPanel;
    [SerializeField] GameObject _inventoryOpenButton;
    [SerializeField] GameObject _inventoryRendersCraft;
    [SerializeField] PlayerInput _playerInput;

    public bool _isOpen = false; 

    public int _totalSlots;
    public int _slotsPerLine;
    public int _slotInVisiblePanel;

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
        _inventoryPanelInvisible.gameObject.SetActive(!_inventoryPanelInvisible.gameObject.activeSelf);
        _inventoryTopPanel.SetActive(!_inventoryTopPanel.gameObject.activeSelf);
        _inventoryOpenButton.SetActive(!_inventoryTopPanel.gameObject.activeSelf);
        if (_isOpen)
        {
            _playerInput.currentActionMap.Enable();
        }
        else
        {
            _playerInput.currentActionMap.Disable();
        }
        _isOpen = !_isOpen;
    }
    public void OpenCloseSpecialPanel(GameObject specialPanel)
    {
        specialPanel.SetActive(!specialPanel.activeSelf);
    }

    public void DisableAllRenderCraft()
    {
        for (int c = 0; c < _inventoryRendersCraft.transform.childCount; c++)
        {
            _inventoryRendersCraft.transform.GetChild(c).gameObject.SetActive(false);
        }
    }
}
