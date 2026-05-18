using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private float hoverTimer = 0f;
    private Transform hoveredPiece = null;

    void Update()
    {
        // Hover check every 0.5 sec
        hoverTimer += Time.deltaTime;
        if (hoverTimer >= 0.5f)
        {
            hoverTimer = 0f;
            Ray hoverRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hoverHits = Physics.RaycastAll(hoverRay, Mathf.Infinity);

            Transform newHover = null;
            foreach (RaycastHit hit in hoverHits)
            {
                if (hit.transform.CompareTag("ChessPiece"))
                {
                    newHover = hit.transform;
                    break;
                }
            }

            // Just started hovering
            if (newHover != null && hoveredPiece == null)
            {
                hoveredPiece = newHover;
                hoveredPiece.position += new Vector3(0, 0.5f, 0);
                print("Is Hover");
            }
            // Stopped hovering
            else if (newHover == null && hoveredPiece != null)
            {
                hoveredPiece.position -= new Vector3(0, 0.5f, 0);
                hoveredPiece = null;
                print("Isn't Hover");
            }
        }



        // Click
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray clickRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] clickHits = Physics.RaycastAll(clickRay, Mathf.Infinity);

            if (clickHits.Length > 0)
            {
                print("Click");
            }
            else
            {
                print("No Click");
            }
        }
    }
}