using UnityEngine;
using UnityEngine.EventSystems;

public class ChessBehaviours : MonoBehaviour
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
        Transform hit = GetPieceUnderMouse();

        
        if (hit != null && _hoveredPiece == null && _selectedPiece == null)
        {
            _hoveredPiece = hit;
            _hoveredPiece.position += Vector3.up * 0.5f;
        }

        
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

        
        if (hit != null && _selectedPiece == null)
        {
            _selectedPiece = hit;
            
            if (_hoveredPiece == null)
                _selectedPiece.position += Vector3.up * 0.5f;
            _hoveredPiece = null;
        }
        
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