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
    
    [NonSerialized] public Item _itemExtraction;

    private Furnace _furnace;
    

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
            StartCoroutine(Extract());
        }
        else if (_isFurnace)
        {
            _furnace = _interfaceBuild.GetComponent<Furnace>();
            _furnace.GetSliderFuel().maxValue = _timeToConsumeFuel;
        }
    }
    public void OnPanelExit()
    {
        _inventory.GetComponent<InventoryUI>().OpenCloseSpecialPanel(_interfaceBuild);
        _inventory.GetComponent<InventoryUI>().OpenInventory();
    }

    private IEnumerator Extract()
    {
        yield return new WaitForSeconds(_timeConsumption);
        _inventory.AddItem(_itemExtraction, 1);
        StartCoroutine(Extract());
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

            if (slotResult._item == null)
            {
                Instantiate(_inventory.GetEmptyItem(), slotResult.transform);
                slotResult._item = slotOre._item._ressourceCook;
            }
            slotResult._quantity++;
            slotResult.GetComponentInChildren<ItemUI>().SetItem(slotResult);

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
}
