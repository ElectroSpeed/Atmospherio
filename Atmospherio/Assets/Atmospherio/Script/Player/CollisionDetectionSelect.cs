using UnityEngine;

public class CollisionDetectionSelect : MonoBehaviour
{
    public bool _collisionEnter;
    private int _numberEnter;
    private void OnTriggerEnter(Collider other)
    {
        _numberEnter += 1;
        _collisionEnter = true;
        print(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        _numberEnter -= 1;
        if (_numberEnter <= 0)
            _collisionEnter = false;
    }
    private void OnEnable()
    {
        _collisionEnter = false;
        _numberEnter = 0;
    }
    private void Update()
    {
        print(_collisionEnter);
        print(_numberEnter);
    }
}
