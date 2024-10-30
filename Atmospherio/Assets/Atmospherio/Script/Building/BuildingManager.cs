using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private DetectBlock _detectBlock;
    [SerializeField] private GameObject _building;

    public static BuildingManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnBuilding()
    {
        Item item = DetectBlock.Instance._blockSelect.GetComponent<Item>();
        GameObject building = Instantiate(_building, _detectBlock._blockSelect.transform.position + Vector3.up, Quaternion.identity);
        building.GetComponent<Building>()._itemExtraction = item;
        
    }
}
