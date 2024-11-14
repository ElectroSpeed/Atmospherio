using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public int _count = 0;
    public TMP_Text _itemCount;
    public Image _itemIcon;
    public Item _item;
    public GameObject _description;

    public void SetItem(InventorySlot slotInformation)
    {
        _count = slotInformation._quantity;
        if(_count > 0)
        {
            _item = slotInformation._item;
            _itemCount.text = slotInformation._quantity.ToString();
            _itemIcon.sprite = slotInformation._item._icon;
            GetComponentInParent<Description>().SetUiDescription(_description);
        }
    }
}
