using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mining : MonoBehaviour
{
    public bool _isMining;
    public float _timeMining;
    public float _currentTimeMining;
    public float _distanceToPlayer;

    private Inventory _inventory;
    private GameObject _currentBlock;

    public Slider _sliderMining;
    private void Start()
    {
        _inventory = FindFirstObjectByType<Inventory>();
        _sliderMining.maxValue = _timeMining;
    }
    public void RightClick(InputAction.CallbackContext ctx)
    {
        _currentBlock = DetectBlock.Instance._blockSelect;
        if (_currentBlock == null)
            return;
        
        if (Vector3.Distance(_currentBlock.transform.position, gameObject.transform.position) > _distanceToPlayer)
            return;

        if (ctx.performed && _currentBlock.CompareTag("Resource"))
        {
            _sliderMining.gameObject.SetActive(true);
            _isMining = true;
        }
        else if (ctx.performed && _currentBlock.CompareTag("Build"))
        {
            if (_inventory.GetComponent<InventoryUI>()._isOpen || _currentBlock.GetComponent<Building>()._interfaceBuild == null)
            {
                return;
            }
            _currentBlock.GetComponent<Building>()._interfaceBuild.SetActive(true);
            _inventory.GetComponent<InventoryUI>().OpenInventory();
        }
        else if (ctx.canceled)
        {
            AudioManager.Instance.StopSFX();
            _sliderMining.gameObject.SetActive(false);
            _currentTimeMining = 0;
            _isMining = false;
        }
    }
    public void LeftClick(InputAction.CallbackContext ctx)
    {
        _currentBlock = DetectBlock.Instance._blockSelect;
        if (_currentBlock == null || !_currentBlock.CompareTag("Build"))
            return;

        if (Vector3.Distance(_currentBlock.transform.position, gameObject.transform.position) > _distanceToPlayer)
            return;
        
        if (_currentBlock.GetComponent<Building>()._itemBuilding == null) 
            return;

        if (ctx.performed)
        {
            _sliderMining.gameObject.SetActive(true);
            _isMining = true;
        }
        else if (ctx.canceled)
        {
            _sliderMining.gameObject.SetActive(false);
            _currentTimeMining = 0;
            _isMining = false;
        }
    }


    private void Update()
    {
        if (!_isMining || _currentBlock == null)
            return;

        if (!_currentBlock.CompareTag("Resource") && !_currentBlock.CompareTag("Build") || _currentBlock != DetectBlock.Instance._blockSelect)
        {
            _currentTimeMining = 0;
            _sliderMining.gameObject.SetActive(false);
            _isMining = false;
            return;
        }

        _currentTimeMining += Time.deltaTime;
        _sliderMining.value = _currentTimeMining;

        if (_currentTimeMining <= 0.1f)
        {
            AudioManager.Instance.PlaySFX("Mining");
        }

        if (_currentTimeMining <= _timeMining)
            return;
        if (_currentBlock.CompareTag("Resource"))
        {
            _inventory.AddItem(_currentBlock.GetComponent<Item>(), 1);
            _currentTimeMining = 0;
            _sliderMining.value = _currentTimeMining;
        }
        else if (_currentBlock.CompareTag("Build") && _currentBlock.GetComponent<Building>()._itemBuilding != null)
        {
            Building building = _currentBlock.GetComponent<Building>();
            _inventory.AddItem(building._itemBuilding, 1);
            _currentTimeMining = 0;
            _sliderMining.value = _currentTimeMining;
            _sliderMining.gameObject.SetActive(false);
            _isMining = false;
            Destroy(_currentBlock);
        }
    }
}
