using TMPro;
using UnityEngine;

public class Life : MonoBehaviour
{
    [SerializeField] private float _life;
    [SerializeField] private float _maxLife;

    [SerializeField] private TextMeshProUGUI _textLife;

    private void Start()
    {
        SetTextLife();
    }

    public void AddLife(float amount)
    {
        _life += amount;
        if (_life > _maxLife)
            _life = _maxLife;

        SetTextLife();
    }
    public void AddMaxLife(float amount)
    {
        _maxLife += amount;
        SetTextLife();
    }
    public void RemoveLife(float amount)
    {
        _life -= amount;
        if (_life < 0)
        {
            _life = 0;
            Debug.Log("Dead");
        }

        SetTextLife();
    }
    public void RemoveMaxLife(float amount)
    {
        _maxLife -= amount;
        SetTextLife();
    }
    public void ResetLife()
    {
        _life = _maxLife;
        SetTextLife();
    }
    public void ResetMaxLife()
    {
        _maxLife = 100f;
        SetTextLife();
    }
    public void Reset()
    {
        ResetLife();
        ResetMaxLife();
    }
    public float GetLife()
    {
        return _life;
    }
    public float GetMaxLife()
    {
        return _maxLife;
    }
    public void SetLife(float amount)
    {
        _life = amount;
        SetTextLife();
    }
    public void SetMaxLife(float amount)
    {
        _maxLife = amount;
        SetTextLife();
    }
    public void SetTextLife()
    {
        _textLife.text = _life.ToString() + " / " + _maxLife.ToString();
    }
}