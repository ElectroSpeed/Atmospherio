using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private DetectBlock _detectBlock;

    public static BuildingManager Instance;
    private CollisionDetectionSelect _collisionDetectionSelect;

    private void Awake()
    {
        Instance = this;
        _collisionDetectionSelect = _detectBlock.GetComponentInChildren<CollisionDetectionSelect>();
    }

    public bool SpawnBuilding(Item itemToBuild)
    {
        Item item = _detectBlock._blockSelect.GetComponent<Item>();
        if ((itemToBuild._itemName == "Extractor" && item == null) || _collisionDetectionSelect._collisionEnter || item == null)
        {
            return false;
        }
        GameObject building = Instantiate(itemToBuild._itemBuilding, _detectBlock._blockSelect.transform.position + Vector3.up, Quaternion.identity);
        building.GetComponent<Building>()._itemExtraction = item;
        return true;
    }
    public void SetSizeCollider(Item itemToBuild)
    {
        
        _collisionDetectionSelect.GetComponent<BoxCollider>().size = itemToBuild._itemBuilding.GetComponent<BoxCollider>().size / 1.25f;
    }
}
