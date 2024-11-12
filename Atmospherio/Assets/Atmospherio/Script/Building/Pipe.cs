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
            print("Pipe enter");
            Pipe otherPipe = other.GetComponent<Pipe>();
            if (other.transform.position.x > _transform.position.x)
            {
                _rightPipe = other.gameObject;
            }
            else if (other.transform.position.x < _transform.position.x)
            {
                _leftPipe = other.gameObject;
            }
        }
        else if (other.gameObject.layer == 7)
        {
            if (other.transform.position.x > _transform.position.x)
            {
                _rightPipe = other.gameObject;
            }
            else if (other.transform.position.x < _transform.position.x)
            {
                _leftPipe = other.gameObject;
            }
        }
    }
}
