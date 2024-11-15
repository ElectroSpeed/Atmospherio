using UnityEngine;
using UnityEngine.EventSystems;

public class Description : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isEnter;
    private Vector3 _mousePos;
    private float _timer;
    private Transform _parent;
    private Transform _transform;
    
    [SerializeField] private float _timeToDisplay;
    [SerializeField] private GameObject _uiDescription;
    private void Awake()
    {
        _transform = transform;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter = true;
        _mousePos = Input.mousePosition;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        print("Exit");
        _isEnter = false;
        if (_uiDescription != null)
            _uiDescription.SetActive(false);
    }
    
    public void SetUiDescription(GameObject uiDescription) => _uiDescription = uiDescription;
    private void Update()
    {
        if (!_isEnter || _uiDescription == null) return;

        if (_timer >= _timeToDisplay && !_uiDescription.activeSelf)
        {
            _uiDescription.transform.position = _mousePos;
            _parent = _uiDescription.transform.parent;
            _uiDescription.transform.SetParent(_transform.root);
            print("True");
            _uiDescription.SetActive(true);
        }
        if (Vector3.Distance(_mousePos, Input.mousePosition) > 0.1f)
        {
            _timer = 0;
            _mousePos = Input.mousePosition;
            if (!_uiDescription.activeSelf)
                return;
            _uiDescription.transform.SetParent(_parent);
            print("False");
            _uiDescription.SetActive(false);
        }
        else
        {
            _timer += Time.deltaTime;
        }
    }
}
