using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBehaviour : MonoBehaviour
{
    private Transform  _selection;
    private RaycastHit _raycastHit;

    [SerializeField] private float liftHeight = 1f;
    [SerializeField] private float liftSpeed  = 5f;

    private Vector3 _originalPosition;
    private bool    _isSelected;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out _raycastHit))
        {
            _selection = _raycastHit.transform;

            if (_selection.CompareTag("Selectable") && Input.GetMouseButtonDown(0))
            {
                _isSelected      = !_isSelected;
                _originalPosition = new Vector3(_selection.position.x, 0.78f, _selection.position.z);
            }
        }

        if (_selection != null)
        {
            Vector3 targetPos = _isSelected
                ? _originalPosition + Vector3.up * liftHeight
                : _originalPosition;

            _selection.position = Vector3.Lerp(_selection.position, targetPos, Time.deltaTime * liftSpeed);
        }
    }
}