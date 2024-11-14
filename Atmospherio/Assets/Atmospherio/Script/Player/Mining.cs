using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mining : MonoBehaviour
{
    public bool _isMining;
    public float _timeMining;
    public float _currentTimeMining;

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
            _sliderMining.gameObject.SetActive(false);
            _currentTimeMining = 0;
            _isMining = false;
        }
    }

    private void Update()
    {
        if (!_isMining || _currentBlock == null)
            return;

        if (!_currentBlock.CompareTag("Resource") || _currentBlock != DetectBlock.Instance._blockSelect)
        {
            _currentTimeMining = 0;
            _sliderMining.gameObject.SetActive(false);
            _isMining = false;
            return;
        }

        _currentTimeMining += Time.deltaTime;
        _sliderMining.value = _currentTimeMining;

        if (_currentTimeMining <= _timeMining)
            return;

        _inventory.AddItem(_currentBlock.GetComponent<Item>(), 1);
        _currentTimeMining = 0;
        _sliderMining.value = _currentTimeMining;
    }
}
