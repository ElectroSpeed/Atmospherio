using UnityEngine;

public class CollisionDetectionSelect : MonoBehaviour
{
    public bool _collisionEnter;
    private int _numberEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 2 || other.gameObject.layer == 9)
            return;

        _numberEnter += 1;
        _collisionEnter = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 2)
            return;

        _numberEnter -= 1;
        if (_numberEnter <= 0)
            _collisionEnter = false;
    }
    private void OnEnable()
    {
        _collisionEnter = false;
        _numberEnter = 0;
    }
}
