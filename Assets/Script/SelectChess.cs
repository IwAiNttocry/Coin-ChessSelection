using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private float hoverTimer = 0f;

    void Update()
    {
        
        hoverTimer += Time.deltaTime;
        if (hoverTimer >= 0.5f)
        {
            hoverTimer = 0f;
            Ray hoverRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hoverHits = Physics.RaycastAll(hoverRay, Mathf.Infinity);

            if (hoverHits.Length > 0)
            {
                print("Is Hover");
            }
            else
            {
                print("Isn't Hover");
            }
        }

        
        
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