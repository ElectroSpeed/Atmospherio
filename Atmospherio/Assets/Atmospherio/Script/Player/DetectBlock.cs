using UnityEngine;
public class DetectBlock : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _posSelect;
    public GameObject _blockSelect;
    public static DetectBlock Instance;
    [HideInInspector] public bool _isDragging;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _camera = Camera.main;
    }
    void Update()
    {
        SetSelection(_posSelect, _camera);
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
                float scaleY = hit.collider.transform.localScale.y / 2;
                posSelect.position =  new Vector3(hit.collider.transform.position.x, 0.5f, hit.collider.transform.position.z);
            }
            
            _blockSelect = hit.collider.gameObject;
        }
    }
}
