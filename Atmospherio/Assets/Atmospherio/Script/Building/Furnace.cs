using UnityEngine;
using UnityEngine.UI;
public class Furnace : MonoBehaviour
{
    [SerializeField] private Slider _sliderFuel;
    [SerializeField] private Slider _sliderCook;
    [SerializeField] private InventorySlot _slotFuel;
    [SerializeField] private InventorySlot _slotOre;
    [SerializeField] private InventorySlot _slotResult;

    public Slider GetSliderFuel() => _sliderFuel;
    public Slider GetSliderCook() => _sliderCook;
    public InventorySlot GetSlotFuel() => _slotFuel;
    public InventorySlot GetSlotOre() => _slotOre;
    public InventorySlot GetSlotResult() => _slotResult;
    
    public void ResetSliderCook()
    {
        _sliderCook.value = 0;
    } 
}