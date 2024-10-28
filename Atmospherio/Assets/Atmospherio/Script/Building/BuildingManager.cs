using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private DetectBlock _detectBlock;
    [SerializeField] private GameObject _building;
    public void LeftClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (_detectBlock._blockSelect != null)
            {
                GameObject building = Instantiate(_building, _detectBlock._blockSelect.transform.position + Vector3.up, Quaternion.identity);
                building.GetComponent<Building>()._itemExtraction = _detectBlock._blockSelect.GetComponent<Item>();
            }
        }
    }
}
