using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ItemUI _itemUI;
    private InventorySlot _slot;
    private InventorySlot _selectedSlot;

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
            _itemUI.transform.SetParent(_itemUI.transform.root);
            _itemUI.transform.SetAsLastSibling();
        }
        if (_itemUI._item.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            DetectBlock.Instance._isDragging = true;
            BuildingManager.Instance.SetSizeCollider(_itemUI._item);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_itemUI == null)
            return;

        _itemUI.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_itemUI == null)
            return;

        if (_itemUI._item.gameObject.layer == LayerMask.NameToLayer("Building"))
        {
            GameObject blockSelect = DetectBlock.Instance._blockSelect;
            if (blockSelect != null)
            {
                DetectBlock.Instance._isDragging = false;
                if (BuildingManager.Instance.SpawnBuilding(_itemUI._item))
                {
                    _itemUI._count -= 1;
                    _itemUI._itemCount.text = _itemUI._count.ToString();
                    if (_itemUI._count <= 0)
                    {
                        Destroy(_itemUI.gameObject);
                        return;
                    }
                }
                    
            }
        }

        InventorySlot targetSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();
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
