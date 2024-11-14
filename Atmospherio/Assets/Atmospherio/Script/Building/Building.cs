using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(Building))]
public class BuildingCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building buildingSystem = target as Building;
        base.OnInspectorGUI();
        SerializeProperty("_isExtraction");
        
        if (buildingSystem._isExtraction)
        {
            SerializeProperty("_timeConsumption");
            SerializeProperty("_timeToConsumeFuel");
        }
        EditorGUILayout.Space(10);
        SerializeProperty("_isFurnace");
        if (buildingSystem._isFurnace)
        {
            SerializeProperty("_timeToConsumeFuel");
        }
        EditorGUILayout.Space(10);
        SerializeProperty("_buildingUI");
        SerializeProperty("_inventory");
        SerializeProperty("_interfaceBuild");
        SerializeProperty("_isChest");
        serializedObject.ApplyModifiedProperties();
    }
    private void SerializeProperty(string variable)
    {
        SerializedProperty serializedProperty = serializedObject.FindProperty(variable);
        EditorGUILayout.PropertyField(serializedProperty);
    }
}
#endif
public class Building : MonoBehaviour
{
    [HideInInspector] public bool _isExtraction;
    [HideInInspector] public bool _isFurnace;
    [HideInInspector] public float _timeConsumption;

    [HideInInspector] public float _timeToConsumeFuel;

    [HideInInspector] public GameObject _buildingUI;
    [HideInInspector] public Inventory _inventory;
    [HideInInspector] public GameObject _interfaceBuild;
    [HideInInspector] public bool _isChest;
    
    [NonSerialized] public Item _itemExtraction;

    private Furnace _furnace;
    private Extractor _extractor;

    public bool _isPiped;
    public GameObject _pipeConnected;
    

    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
        if(_buildingUI != null)
        {
            _interfaceBuild = Instantiate(_buildingUI, _inventory.transform);
            _interfaceBuild.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
            var specialButton = _interfaceBuild.transform.GetChild(0).GetComponent<SpecialButton>();
            specialButton._onClick.AddListener(OnPanelExit);
        }
        if (_isExtraction)
        {
            _extractor = _interfaceBuild.GetComponent<Extractor>();
            _extractor.GetSliderFuel().maxValue = _timeToConsumeFuel;
            _extractor.GetSliderExtraction().maxValue = _timeConsumption;
        }
        else if (_isFurnace)
        {
            _furnace = _interfaceBuild.GetComponent<Furnace>();
            _furnace.GetSliderFuel().maxValue = _timeToConsumeFuel;
        }
    }
    public void SetPiped(bool piped, GameObject pipe)
    {
        if (_isPiped) return;
        
        _isPiped = piped;
        _pipeConnected = pipe;
    }
    public void OnPanelExit()
    {
        _inventory.GetComponent<InventoryUI>().OpenCloseSpecialPanel(_interfaceBuild);
        _inventory.GetComponent<InventoryUI>().OpenInventory();
    }

    private void CookRessource()
    {
        InventorySlot slotOre = _furnace.GetSlotOre();
        InventorySlot slotFuel = _furnace.GetSlotFuel();
        InventorySlot slotResult = _furnace.GetSlotResult();
        Slider sliderCook = _furnace.GetSliderCook();
        Slider sliderFuel = _furnace.GetSliderFuel();

        if (slotOre._item != null && slotOre._item._timeCook > 0)
        {
            sliderCook.maxValue = slotOre._item._timeCook;
        }

        if (slotResult._item != null && slotOre._item._ressourceCook != slotResult._item)
        {
            _furnace.ResetSliderCook();
            sliderFuel.value = Mathf.Max(0, sliderFuel.value - Time.deltaTime);
            return;
        }

        if (sliderCook.value >= sliderCook.maxValue)
        {
            sliderCook.value = 0;
            slotOre._quantity--;
            slotOre.GetComponentInChildren<ItemUI>().SetItem(slotOre);

            if (!TryPipe())
            {
                if (slotResult._item == null)
                {
                    Instantiate(_inventory.GetEmptyItem(), slotResult.transform);
                    slotResult._item = slotOre._item._ressourceCook;
                }
                slotResult._quantity++;
                slotResult.GetComponentInChildren<ItemUI>().SetItem(slotResult);
            }

            if (slotOre._quantity <= 0)
            {
                slotOre._item = null;
                Destroy(slotOre.transform.GetChild(0).gameObject);
            }
        }

        if (sliderFuel.value <= 0)
        {
            if (slotFuel._item != null && slotFuel._item._itemName == "Coal")
            {
                slotFuel._quantity--;
                slotFuel.GetComponentInChildren<ItemUI>().SetItem(slotFuel);

                if (slotFuel._quantity > 0)
                {
                    sliderFuel.value = _timeToConsumeFuel;
                }
                else
                {
                    slotFuel._item = null;
                }
            }
        }

        sliderFuel.value = Mathf.Max(0, sliderFuel.value - Time.deltaTime);
        sliderCook.value = Mathf.Min(sliderCook.maxValue, sliderCook.value + Time.deltaTime);
    }

    private void Update()
    {
        if (_isFurnace)
        {
            TryCookRessource();
        }
        if (_isExtraction)
        {
            TryExtractRessource();
        }
    }

    private bool TryExtractRessource()
    {
        if (_extractor.GetSlotFuel()._item == null || _extractor.GetSlotFuel()._item._itemName != "Coal")
        {
            _extractor.GetSliderFuel().value -= Time.deltaTime;
            if (_extractor.GetSliderFuel().value <= 0)
            {
                _extractor.ResetSliderExtraction();
                return false;
            }
            else
            {
                ExtractRessource();
                return true;
            }
        }
        if (_extractor.GetSlotFuel()._item != null)
        {
            ExtractRessource();
            Debug.Log("Extract");
            return true;
        }
        Debug.Log("Sortie");
        return false;
    }

    private void ExtractRessource()
    {
        InventorySlot slotFuel = _extractor.GetSlotFuel();
        InventorySlot slotResult = _extractor.GetSlotResult();
        Slider sliderExtraction = _extractor.GetSliderExtraction();
        Slider sliderFuel = _extractor.GetSliderFuel();

        if (sliderFuel.value <= 0)
        {
            sliderFuel.value = _timeToConsumeFuel;
            slotFuel._quantity--;
            slotFuel.GetComponentInChildren<ItemUI>().SetItem(slotFuel);
        }        
        if (sliderExtraction.value >= sliderExtraction.maxValue)
        {
            if (!TryPipe())
            {
                if (slotResult._item == null)
                {
                    Instantiate(_inventory.GetEmptyItem(), slotResult.transform);
                    slotResult._item = _itemExtraction;
                }
                sliderExtraction.value = 0;
                slotResult._quantity++;
                slotResult.GetComponentInChildren<ItemUI>().SetItem(slotResult);  
            }
        }
        sliderExtraction.value = Mathf.Min(sliderExtraction.maxValue, sliderExtraction.value + Time.deltaTime);
        sliderFuel.value = Mathf.Max(0, sliderFuel.value - Time.deltaTime);

    }

    private bool TryCookRessource()
    {
        if (_furnace.GetSlotFuel()._item == null || _furnace.GetSlotFuel()._item._itemName != "Coal")
        {
            _furnace.GetSliderFuel().value -= Time.deltaTime;
            if (_furnace.GetSliderFuel().value <= 0)
            {
                _furnace.ResetSliderCook();
            }
            else if (_furnace.GetSlotOre()._item != null && _furnace.GetSlotOre()._item._timeCook != 0)
            {
                CookRessource();
                return true;
            }
            else
            {
                _furnace.ResetSliderCook();
                return false; 
            }
        }
        if (_furnace.GetSlotOre()._item == null || _furnace.GetSlotOre()._item._ressourceCook == null || _furnace.GetSlotOre()._item._timeCook <= 0)
        {
            _furnace.GetSliderFuel().value -= Time.deltaTime;
            _furnace.ResetSliderCook();
            return false;
        }
        
        CookRessource();
        return true;
    }
    private bool TryPipe()
    {
        if (_pipeConnected == null || !_isFurnace && !_isExtraction || _isChest) return false;

        Pipe pipe = _pipeConnected.GetComponent<Pipe>();
        GameObject pipeConnected = gameObject;

        for (int i = 0; i < 9999; i++)
        {
            if (pipe._rightPipe != null && pipe._rightPipe != pipeConnected)
            {
                pipeConnected = pipe.gameObject;
                if (pipe._rightPipe.GetComponent<Building>() != null)
                {
                    return AddWithPipe(pipe._rightPipe.GetComponent<Building>());
                }
                pipe = pipe._rightPipe.GetComponent<Pipe>();
            }
            else if (pipe._leftPipe != pipeConnected && pipe._leftPipe != null)
            {
                pipeConnected = pipe.gameObject;
                pipe = pipe._leftPipe.GetComponent<Pipe>();
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    private bool AddWithPipe(Building building)
    {
        print("enter");
        if (_furnace)
        {
            if (building._isChest)
            {
                _inventory.AddItemInChest(_furnace.GetSlotOre()._item._ressourceCook, 1, building._interfaceBuild);
                return true;
            }
        }
        else if (_isExtraction)
        {
            if (building._isFurnace)
            {
                if (_itemExtraction ._itemName == "Coal" && (building._furnace.GetSlotFuel()._item  == null  ||
                      building._furnace.GetSlotFuel()._quantity < building._furnace.GetSlotFuel()._item._maxStack))
                {
                    InstantiateSlot(building, building._furnace.GetSlotFuel(), _itemExtraction);
                    _extractor.GetSliderExtraction().value = 0;
                    return true;
                }
                else if (_itemExtraction ._itemName != "Coal" && building._furnace.GetSlotOre()._item == null || _itemExtraction == building._furnace.GetSlotOre()._item && 
                         building._furnace.GetSlotOre()._quantity < building._furnace.GetSlotOre()._item._maxStack)
                {
                    InstantiateSlot(building, building._furnace.GetSlotOre(), _itemExtraction);
                    _extractor.GetSliderExtraction().value = 0;
                    return true;
                }
                return false;
            }
            else if (building._isChest)
            {
                _inventory.AddItemInChest(_itemExtraction, 1, building._interfaceBuild);
                _extractor.GetSliderExtraction().value = 0;
                return true;
            }
        }
        return false;
    }

    private void InstantiateSlot(Building building, InventorySlot slot, Item item)
    {
        if (slot._item == null)
        {
            GameObject obj = Instantiate(_inventory.GetEmptyItem(), slot.transform);
            slot._item = item;
        }
        slot._quantity++;
        slot.GetComponentInChildren<ItemUI>().SetItem(slot);
    }
}
