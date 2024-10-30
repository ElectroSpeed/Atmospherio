using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemUI _itemUI;
    private InventorySlot _slot;
    private InventorySlot _selectedSlot;
    private Transform _parentBeforeDrag;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (this.transform.childCount > 0 && this.transform.GetChild(0).GetComponent<ItemUI>())
        {
            _itemUI = this.transform.GetChild(0).GetComponent<ItemUI>();
        }
        _slot = GetComponent<InventorySlot>();

        if (_itemUI != null)
        {
            _slot.ResetSlot();
            _selectedSlot = _slot;
            _itemUI.transform.parent = _itemUI.transform.root;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _itemUI.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventorySlot targetSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();
        Debug.Log(targetSlot);
        if (targetSlot != null)
        {
            if (targetSlot.transform.childCount == 0)
            {
                _itemUI.transform.parent = targetSlot.transform;
                _itemUI.transform.position = targetSlot.transform.position;
                targetSlot.SetSlot(_itemUI);
            }
            else
            {
                Debug.Log("Enter");
                SwitchSlot(_slot, targetSlot);
            }
        }
        else
        {
            _itemUI.transform.parent = _selectedSlot.transform;
            _itemUI.transform.position = _selectedSlot.transform.position;
            _selectedSlot.SetSlot(_itemUI);
        }
    }

    private void SwitchSlot(InventorySlot baseSlot, InventorySlot targetSlot)
    {
        ItemUI tempItem = targetSlot.transform.GetChild(0).GetComponent<ItemUI>();

        tempItem.transform.SetParent(baseSlot.transform);
        tempItem.transform.position = baseSlot.transform.position;
        baseSlot.SetSlot(tempItem);

        _itemUI.transform.SetParent(targetSlot.transform);
        _itemUI.transform.position = targetSlot.transform.position;
        targetSlot.SetSlot(_itemUI);
    }
}
