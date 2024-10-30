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
        if (ctx.performed && DetectBlock.Instance._blockSelect.CompareTag("Resource"))
        {
            _sliderMining.gameObject.SetActive(true);
            _currentBlock = DetectBlock.Instance._blockSelect;
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
