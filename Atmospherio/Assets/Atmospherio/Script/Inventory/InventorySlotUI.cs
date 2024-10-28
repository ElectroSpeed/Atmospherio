using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image _itemIcon;
    public TMP_Text _quantityText;
    public InventorySlot _linkedSlot;

    private InventoryUI _inventoryUI;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public void SetupSlotUI(InventorySlot slot, InventoryUI ui)
    {
        _linkedSlot = slot;
        _inventoryUI = ui;
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        UpdateSlotUI(slot);
    }

    public void UpdateSlotUI(InventorySlot slot)
    {
        _linkedSlot = slot;

        if (slot != null && slot._item != null)
        {
            _itemIcon.sprite = slot._item._icon;
            _itemIcon.enabled = true;
            _quantityText.text = slot._quantity > 1 ? slot._quantity.ToString() : "";
        }
        else
        {
            ClearSlotUI();
        }
    }

    public void ClearSlotUI()
    {
        _itemIcon.sprite = null;
        _itemIcon.enabled = false;
        _quantityText.text = "";
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_linkedSlot == null || _linkedSlot._item == null) return;

        _canvasGroup.alpha = 0.6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_linkedSlot == null || _linkedSlot._item == null) return;

        _rectTransform.anchoredPosition += eventData.delta / _inventoryUI.GetCanvas().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;

        InventorySlotUI targetSlotUI = eventData.pointerEnter?.GetComponent<InventorySlotUI>();

        if (targetSlotUI != null && targetSlotUI != this)
        {
            if (targetSlotUI._linkedSlot != null && targetSlotUI._linkedSlot._item == _linkedSlot._item)
            {
                int availableSpace = targetSlotUI._linkedSlot._item._maxStack - targetSlotUI._linkedSlot._quantity;
                int transferAmount = Mathf.Min(_linkedSlot._quantity, availableSpace);

                targetSlotUI._linkedSlot._quantity += transferAmount;
                _linkedSlot._quantity -= transferAmount;

                if (_linkedSlot._quantity <= 0)
                {
                    _linkedSlot._item = null;
                    _linkedSlot._quantity = 0;
                }
                _inventoryUI.UpdateUI();
            }
            else
            {
                InventorySlot tempSlot = new InventorySlot(_linkedSlot._item, _linkedSlot._quantity);
                _linkedSlot._item = targetSlotUI._linkedSlot._item;
                _linkedSlot._quantity = targetSlotUI._linkedSlot._quantity;

                targetSlotUI._linkedSlot._item = tempSlot._item;
                targetSlotUI._linkedSlot._quantity = tempSlot._quantity;

                _inventoryUI.UpdateUI();
            }
        }
        else
        {
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
