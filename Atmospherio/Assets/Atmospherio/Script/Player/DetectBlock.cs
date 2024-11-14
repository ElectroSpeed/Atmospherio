using UnityEngine;
using UnityEngine.InputSystem;
public class DetectBlock : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _posSelect;
    public GameObject _blockSelect;
    public static DetectBlock Instance;
    [HideInInspector] public bool _isDragging;
    public float _distanceToPlayer;
    private SpriteRenderer _selectSprite;
    private Transform _transform;

    private void Awake()
    {
        Instance = this;
        _selectSprite = _posSelect.GetComponentInChildren<SpriteRenderer>();
        _transform = transform;
    }
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        SetSelection(_posSelect, _camera);

        _selectSprite.color = Vector3.Distance(_transform.position, _posSelect.position) > _distanceToPlayer ? Color.red : Color.white;
    }

    private void SetSelection(Transform posSelect, Camera camera)
    {
        Vector3 posMouse = camera.ScreenToWorldPoint(Input.mousePosition);
        Debug.DrawLine(posMouse, posMouse + new Vector3(0, -1, 1) * 100, Color.red);
        if (Physics.Raycast(posMouse, new Vector3(0, -1, 1), out RaycastHit hit, 1000f))
        {
            if ((hit.collider.CompareTag("BlockBasic") || hit.collider.gameObject.layer == 6) && !_isDragging)
            {
                posSelect.gameObject.SetActive(false);
            }
            else
            {
                posSelect.gameObject.SetActive(true);
                if (hit.collider.gameObject.layer == 7 && (!hit.collider.GetComponent<Building>()._isChest && !hit.collider.GetComponent<Building>()._isFurnace && !hit.collider.GetComponent<Building>()._isExtraction))
                {
                    posSelect.position = new Vector3(hit.collider.transform.position.x, 1.5f, hit.collider.transform.position.z);
                }
                else
                    posSelect.position =  new Vector3(hit.collider.transform.position.x, 0.5f, hit.collider.transform.position.z);
            }
            
            _blockSelect = hit.collider.gameObject;
        }
    }
    public void RotateBlock(InputAction.CallbackContext ctx)
    {
        if (!ctx.canceled) return;
        if (_blockSelect == null) return;
        Pipe pipe = _blockSelect.GetComponentInChildren<Pipe>();
        if (pipe == null) return;
        pipe.Rotate();
    }
}
