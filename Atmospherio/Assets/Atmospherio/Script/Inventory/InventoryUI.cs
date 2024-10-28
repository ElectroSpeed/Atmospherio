using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Settings")]
    public GameObject _inventorySlotPrefab;
    public Transform _inventoryPanel;
    public int _totalSlots = 20;
    public int _slotsPerRow = 5;
    public Vector2 _slotSize = new Vector2(100, 100);
    public float _slotSpacing = 10f;

    private Inventory _inventory;
    private List<InventorySlotUI> slotUIs = new List<InventorySlotUI>();

    private void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
        GenerateInventoryGrid();
        UpdateUI();
    }

    private void GenerateInventoryGrid()
    {
        int rows = Mathf.CeilToInt((float)_totalSlots / _slotsPerRow);

        float gridWidth = (_slotsPerRow * _slotSize.x) + ((_slotsPerRow - 1) * _slotSpacing);
        float gridHeight = (rows * _slotSize.y) + ((rows - 1) * _slotSpacing);

        Vector2 startPosition = new Vector2(-gridWidth / 2 + _slotSize.x / 2, gridHeight / 2 - _slotSize.y / 2);

        for (int i = 0; i < _totalSlots; i++)
        {
            int row = i / _slotsPerRow;
            int col = i % _slotsPerRow;

            Vector2 slotPosition = startPosition + new Vector2(col * (_slotSize.x + _slotSpacing), -row * (_slotSize.y + _slotSpacing));

            GameObject slotUI = Instantiate(_inventorySlotPrefab, _inventoryPanel);
            slotUI.GetComponent<RectTransform>().anchoredPosition = slotPosition;

            InventorySlotUI slotScript = slotUI.GetComponent<InventorySlotUI>();
            slotScript.SetupSlotUI(null, this);
            slotUIs.Add(slotScript);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < slotUIs.Count; i++)
        {
            if (i < _inventory._slots.Count)
            {
                slotUIs[i].UpdateSlotUI(_inventory._slots[i]);
            }
            else
            {
                slotUIs[i].ClearSlotUI();
            }
        }
    }
    public Canvas GetCanvas()
    {
        return GetComponentInParent<Canvas>();
    }
}
