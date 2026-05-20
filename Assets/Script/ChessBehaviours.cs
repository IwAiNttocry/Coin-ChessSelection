using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Transform _hoveredPiece;
    private Transform _selectedPiece;
    private Vector2Int _lastSquare = -Vector2Int.one;
    private Vector3 _lastMousePos;

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

        //Option 4 — only run when mouse enters a new square
        //Vector2Int currentSquare = GetSquareUnderMouse();
        //if (currentSquare == _lastSquare) return;
        //_lastSquare = currentSquare;

        Transform hit = GetPieceUnderMouse();

        // Mouse entered a piece
        if (hit != null && _hoveredPiece == null)
        {
            _hoveredPiece = hit;
            _hoveredPiece.position += Vector3.up * 0.5f;
        }

        // Mouse left the piece
        if (hit == null && _hoveredPiece != null)
        {
            _hoveredPiece.position -= Vector3.up * 0.5f;
            _hoveredPiece = null;
        }
    }

    void CheckClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Transform hit = GetPieceUnderMouse();

        // Select
        if (hit != null && _selectedPiece == null)
        {
            _selectedPiece = hit;
            if (_hoveredPiece == null)
                _selectedPiece.position += Vector3.up * 0.5f;
            _hoveredPiece = null;
        }
        // Deselect
        else if (_selectedPiece != null)
        {
            _selectedPiece.position -= Vector3.up * 0.5f;
            _selectedPiece = null;
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