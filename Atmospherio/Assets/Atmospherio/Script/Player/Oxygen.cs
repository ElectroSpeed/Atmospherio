using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [SerializeField] private float _oxygen;
    [SerializeField] private float _maxOxygen;
    [SerializeField] private float _speedRemoveOxygen;

    [SerializeField] private Slider _oxygenSlider;
    private Life _life;

    private void Awake()
    {
        _life = GetComponent<Life>();
    }

    private void Start()
    {
        SetSlider();
    }

    private void Update()
    {
        RemoveOxygen(_speedRemoveOxygen * Time.deltaTime);
    }

    public void AddOxygen(float amount)
    {
        _oxygen += amount;
        if (_oxygen > _maxOxygen)
            _oxygen = _maxOxygen;

        SetSlider();
    }
    public void AddMaxOxygen(float amount)
    {
        _maxOxygen += amount;
        SetSlider();
    }
    public void RemoveOxygen(float amount)
    {
        _oxygen -= amount;
        if (_oxygen < 0)
        {
            _life.RemoveLife(0.1f);
            _oxygen = 0;
        }
        SetSlider();
    }
    public void RemoveMaxOxygen(float amount)
    {
        _maxOxygen -= amount;
        SetSlider();
    }
    public float GetOxygen()
    {
        return _oxygen;
    }
    public float GetMaxOxygen()
    {
        return _maxOxygen;
    }
    public void SetOxygen(float amount)
    {
        _oxygen = amount;
        SetSlider();
    }
    public void SetMaxOxygen(float amount)
    {
        _maxOxygen = amount;
        SetSlider();
    }
    public void ResetOxygen()
    {
        _oxygen = _maxOxygen;
        SetSlider();
    }
    public void ResetMaxOxygen()
    {
        _maxOxygen = 100f;
        SetSlider();
    }
    public void Reset()
    {
        ResetOxygen();
        ResetMaxOxygen();
    }

    public void SetSlider()
    {
        _oxygenSlider.value = _oxygen;
        _oxygenSlider.maxValue = _maxOxygen;
    }
}