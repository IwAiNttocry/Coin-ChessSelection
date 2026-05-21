using UnityEngine;

public class ChessBehaviour : MonoBehaviour
{
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private ChessSelector selector;

    private static readonly Vector3 LiftOffset = Vector3.up * 0.5f;

    
    public void OnHoverEnter()
    {
        transform.position += LiftOffset;
    }

    
    public void OnHoverExit()
    {
        transform.position -= LiftOffset;
    }

    
    public void OnSelect(bool alreadyLifted)
    {
        if (!alreadyLifted)
            transform.position += LiftOffset;

        Debug.Log($"{name} selected");
    }

        public void OnDeselect()
    {
        transform.position -= LiftOffset;
        Debug.Log($"{name} deselected");
    }

    
    public void OnDoubleClick()
    {
        Debug.Log($"{name} double-clicked");
        
    }
}