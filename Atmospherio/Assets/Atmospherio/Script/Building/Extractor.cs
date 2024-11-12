using UnityEngine;
using UnityEngine.UI;
public class Extractor : MonoBehaviour
{
    [SerializeField] private Slider _sliderFuel;
    [SerializeField] private Slider _sliderExtraction;
    [SerializeField] private InventorySlot _slotFuel;
    [SerializeField] private InventorySlot _slotResult;

    public Slider GetSliderFuel() => _sliderFuel;
    public Slider GetSliderExtraction() => _sliderExtraction;
    public InventorySlot GetSlotFuel() => _slotFuel;
    public InventorySlot GetSlotResult() => _slotResult;
    
    public void ResetSliderExtraction()
    {
        _sliderExtraction.value = 0;
    } 
}