using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Transform _hoveredPiece;
    private Transform _selectedPiece;
    private Vector2Int _lastSquare = -Vector2Int.one;
    private Vector3 _lastMousePos;

    private float _doubleClickTimer = 0f;
    private float _doubleClickThreshold = 0.3f;
    private bool _waitingForDoubleClick = false;

    void Update()
    {
        CheckHover();
        CheckClick();
    }

    void CheckHover()
    {
        if (_selectedPiece != null) return;

        if (Vector3.Distance(Input.mousePosition, _lastMousePos) < 5f) return;
        _lastMousePos = Input.mousePosition;

        Vector2Int currentSquare = GetSquareUnderMouse();
        if (currentSquare == _lastSquare) return;
        _lastSquare = currentSquare;

        Transform hit = GetPieceUnderMouse();

        if (hit != null && _hoveredPiece == null)
        {
            _hoveredPiece = hit;
            _hoveredPiece.position += Vector3.up * 0.5f;
        }

        if (hit == null && _hoveredPiece != null)
        {
            _hoveredPiece.position -= Vector3.up * 0.5f;
            _hoveredPiece = null;
        }
    }

    void CheckClick()
    {
        // Right click — deselect
        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedPiece != null)
            {
                _selectedPiece.position -= Vector3.up * 0.5f;
                _selectedPiece = null;
            }
            return;
        }

        // Left click
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Transform hit = GetPieceUnderMouse();

        // Double click check
        if (_waitingForDoubleClick && hit == _selectedPiece)
        {
            // Second click landed within threshold on the same piece
            _waitingForDoubleClick = false;
            _doubleClickTimer = 0f;
            Debug.Log("Double Click");
            return;
        }

        // Select
        if (hit != null && _selectedPiece == null)
        {
            _selectedPiece = hit;
            if (_hoveredPiece == null)
                _selectedPiece.position += Vector3.up * 0.5f;
            _hoveredPiece = null;

            // Start waiting to see if a second click comes
            _waitingForDoubleClick = true;
            _doubleClickTimer = 0f;
        }

        // Tick double click timer
        if (_waitingForDoubleClick)
        {
            _doubleClickTimer += Time.deltaTime;
            if (_doubleClickTimer >= _doubleClickThreshold)
            {
                _waitingForDoubleClick = false;
                _doubleClickTimer = 0f;
            }
        }
    }

    Vector2Int GetSquareUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            return new Vector2Int(
                Mathf.FloorToInt(hit.point.x),
                Mathf.FloorToInt(hit.point.z)
            );
        return -Vector2Int.one;
    }

    Transform GetPieceUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
            if (hit.transform.CompareTag("ChessPiece"))
                return hit.transform;
        return null;
    }
}