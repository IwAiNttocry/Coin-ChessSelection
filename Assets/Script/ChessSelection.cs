using UnityEngine;
using UnityEngine.EventSystems;

public class ChessSelection : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private ChessBehaviour _hoveredPiece;
    private ChessBehaviour _selectedPiece;
    private Vector2Int _lastSquare = -Vector2Int.one;
    private Vector3 _lastMousePos;

    private float _doubleClickTimer = 0f;
    private const float DoubleClickThreshold = 0.3f;
    private bool _waitingForDoubleClick = false;

    void Update()
    {
        CheckHover();
        CheckClick();
        TickDoubleClickTimer();
    }

    // ─── Hover ────────────────────────────────────────────────────────────────

    void CheckHover()
    {
        if (_selectedPiece != null) return;
        if (Vector3.Distance(Input.mousePosition, _lastMousePos) < 5f) return;

        _lastMousePos = Input.mousePosition;

        Vector2Int currentSquare = GetSquareUnderMouse();
        if (currentSquare == _lastSquare) return;
        _lastSquare = currentSquare;

        ChessBehaviour hit = GetPieceUnderMouse();

        if (hit != null && _hoveredPiece == null)
        {
            _hoveredPiece = hit;
            _hoveredPiece.OnHoverEnter();
        }

        if (hit == null && _hoveredPiece != null)
        {
            _hoveredPiece.OnHoverExit();
            _hoveredPiece = null;
        }
    }

    // ─── Click ────────────────────────────────────────────────────────────────

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (_selectedPiece != null)
            {
                _selectedPiece.OnDeselect();
                _selectedPiece = null;
            }
            return;
        }

        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        ChessBehaviour hit = GetPieceUnderMouse();

        if (_waitingForDoubleClick && hit == _selectedPiece)
        {
            _waitingForDoubleClick = false;
            _doubleClickTimer = 0f;
            _selectedPiece.OnDoubleClick();
            return;
        }

        if (hit != null && _selectedPiece == null)
        {
            bool wasHovered = (_hoveredPiece == hit);
            _selectedPiece = hit;
            _selectedPiece.OnSelect(wasHovered);
            _hoveredPiece = null;

            _waitingForDoubleClick = true;
            _doubleClickTimer = 0f;
        }
    }

    void TickDoubleClickTimer() // Wait for The Click
    {
        if (!_waitingForDoubleClick) return;
        _doubleClickTimer += Time.deltaTime;
        if (_doubleClickTimer >= DoubleClickThreshold)
        {
            _waitingForDoubleClick = false;
            _doubleClickTimer = 0f;
        }
    }

    // ─── Raycasts ─────────────────────────────────────────────────────────────

    Vector2Int GetSquareUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            return new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
        return -Vector2Int.one;
    }

    ChessBehaviour GetPieceUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
        {
            ChessBehaviour piece = hit.transform.GetComponent<ChessBehaviour>();
            if (piece != null) return piece;
        }
        return null;
    }
}
