using System.Collections;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Building))]
public class BuildingCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Building buildingSystem = target as Building;
        base.OnInspectorGUI();

        if (buildingSystem._isExtraction)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty("_timeConsumption");
            EditorGUILayout.PropertyField(serializedProperty);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif
public class Building : MonoBehaviour
{
    public bool _isExtraction;
    [SerializeField] private GameObject _buildingUI;

    [HideInInspector] public float _timeConsumption;

    [HideInInspector] public Item _itemExtraction;
    public Inventory _inventory;
    public GameObject _interfaceBuild;
    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
        if(_buildingUI != null)
        {
            _interfaceBuild = Instantiate(_buildingUI);
            _interfaceBuild.transform.SetParent(_inventory.transform);
            _interfaceBuild.GetComponent<RectTransform>().localPosition = new Vector3(0, 150, 0);
        }
        if (_isExtraction)
        {
            StartCoroutine(Extract());
        }
    }
    private IEnumerator Extract()
    {
        yield return new WaitForSeconds(_timeConsumption);
        _inventory.AddItem(_itemExtraction, 1);
        StartCoroutine(Extract());
    }
}