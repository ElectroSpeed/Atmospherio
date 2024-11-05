using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    private Light _light;
    private Transform _transform;
    public int _levelZone = 0;
    private Oxygen _oxygenPlayer;
    private bool _playerExit;
    private void Start()
    {
        _light = GetComponentInChildren<Light>();
        _transform = transform;
        UpgradeZone(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _oxygenPlayer = other.GetComponent<Oxygen>();
            _oxygenPlayer.ResetOxygen();
            _playerExit = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerExit = true;
        }
    }

    public void UpgradeZone(int number)
    {
        _levelZone += number;
        _transform.localScale = (_levelZone + 1) * 10 * Vector3.one;
        _light.intensity = _transform.localScale.x * 200f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            UpgradeZone(1);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            UpgradeZone(-1);
        }
        if (_playerExit)
        {
            _oxygenPlayer.RemoveOxygen(_oxygenPlayer._speedRemoveOxygen * Time.deltaTime);
        }
    }
}
