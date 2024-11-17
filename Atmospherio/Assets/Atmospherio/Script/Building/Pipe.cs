using UnityEngine;
public class Pipe : MonoBehaviour
{
    private Transform _transform;
    public GameObject _leftPipe;
    public GameObject _rightPipe;
    private void Start()
    {
        _transform = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            SetLeftRightPipe(other.transform);
        }
        else if (other.gameObject.layer == 7)
        {
            SetLeftRightPipe(other.transform);
            other.GetComponent<Building>().SetPiped(gameObject);
        }
    }
    private void SetLeftRightPipe(Transform other)
    {
        if (other.position.x > _transform.position.x)
        {
            _rightPipe = other.gameObject;
        }
        else if (other.position.x < _transform.position.x)
        {
            _leftPipe = other.gameObject;
        }
        else if (other.position.z > _transform.position.z)
        {
            _rightPipe = other.gameObject;
        }
        else if (other.position.z < _transform.position.z)
        {
            _leftPipe = other.gameObject;
        }
    }
    public void Rotate()
    {
        if (_leftPipe != null && _leftPipe.GetComponent<Pipe>() != null)
        {
            _leftPipe.GetComponent<Pipe>()._rightPipe = null;
        }
        if (_rightPipe != null && _rightPipe.GetComponent<Pipe>() != null)
        {
            _rightPipe.GetComponent<Pipe>()._leftPipe = null;
        }
        _leftPipe = null;
        _rightPipe = null;
        transform.parent.Rotate(0, 90, 0);
    }
}
