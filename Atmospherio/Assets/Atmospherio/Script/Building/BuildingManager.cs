using UnityEngine;
using UnityEngine.EventSystems;

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
    private bool IsMouseOverUI() {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public bool SpawnBuilding(Item itemToBuild)
    {
        Item item = _detectBlock._blockSelect.GetComponent<Item>();
        if ((itemToBuild._itemName == "Extractor" && item == null) || _collisionDetectionSelect._collisionEnter || itemToBuild._itemBuilding == null || IsMouseOverUI())
        {
            return false;
        }
        GameObject building = Instantiate(itemToBuild._itemBuilding, _detectBlock._blockSelect.transform.position + Vector3.up, Quaternion.identity);
        building.GetComponent<Building>()._itemExtraction = item;
        return true;
    }
    public void SetSizeCollider(Item itemToBuild)
    {
        if (itemToBuild._itemBuilding == null) return;
        
        _collisionDetectionSelect.GetComponent<BoxCollider>().size = itemToBuild._itemBuilding.GetComponent<BoxCollider>().size / 1.25f;
    }
}
