using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed;
    private Rigidbody _rb;
    private Vector3 _direction;

    private Camera _camera;
    [SerializeField] private float _maxSizeCamera;
    [SerializeField] private float _minSizeCamera;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>();
    }
    private void FixedUpdate()
    {
        if (_direction != Vector3.zero)
            _rb.linearVelocity = _walkSpeed * Time.fixedDeltaTime * _direction;
    }
    public void Zoom(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            float zoom = ctx.ReadValue<float>();

            float newSize = _camera.orthographicSize - zoom;
            if (newSize < _minSizeCamera || newSize > _maxSizeCamera)
                return;

            _camera.orthographicSize -= zoom;
        }
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            _direction = new Vector3(ctx.ReadValue<Vector2>().x, 0, ctx.ReadValue<Vector2>().y);

        else if (ctx.canceled)
        {
            _rb.linearVelocity = Vector3.zero;
            _direction = Vector3.zero;
        }
    }
}