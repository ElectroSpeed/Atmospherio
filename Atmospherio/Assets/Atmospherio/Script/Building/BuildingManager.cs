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

    public bool SpawnBuilding(Item itemToBuild)
    {
        Item item = DetectBlock.Instance._blockSelect.GetComponent<Item>();
        if (itemToBuild._itemName == "Extractor" && item == null)
        {
            return false;
        }
        GameObject building = Instantiate(itemToBuild._itemBuilding, _detectBlock._blockSelect.transform.position + Vector3.up, Quaternion.identity);
        building.GetComponent<Building>()._itemExtraction = item;
        return true;
    }
}
