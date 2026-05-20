using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    private Transform _hoveredPiece;
    private Transform _selectedPiece;
    private float _timer;

    void Update()
    {
        CheckHover();
        CheckClick();
    }

    void CheckHover()
    {
        _timer += Time.deltaTime;
        if (_timer < 0.5f) return;
        _timer = 0f;

        Transform hit = GetPieceUnderMouse();

        // Mouse entered a piece
        if (hit != null && _hoveredPiece == null && _selectedPiece == null)
        {
            _hoveredPiece = hit;
            _hoveredPiece.position += Vector3.up * 0.5f;
        }

        // Mouse left the piece
        if (hit == null && _hoveredPiece != null && _selectedPiece == null)
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
            // If not already lifted by hover, lift it now
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

    Transform GetPieceUnderMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Mathf.Infinity))
            if (hit.transform.CompareTag("ChessPiece"))
                return hit.transform;
        return null;
    }
}