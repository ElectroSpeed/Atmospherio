using UnityEngine;
public class DetectBlock : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private Transform _posSelect;
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

        if (Physics.Raycast(posMouse, new Vector3(0, -1, 0), out RaycastHit hit, 1000f))
        {
            if (!hit.collider.CompareTag("Resource"))
                posSelect.gameObject.SetActive(false);
            else
            {
                posSelect.gameObject.SetActive(true);
                posSelect.position = hit.collider.transform.position + new Vector3(0, 0.52f, 0);
            }
        }
    }
}
