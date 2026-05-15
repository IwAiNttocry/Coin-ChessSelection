using UnityEngine;

public class ClickObject : MonoBehaviour
{
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.Log("Hit: " + hit.collider.gameObject.name + " | Tag: " + hit.collider.gameObject.tag);

                if (hit.collider.CompareTag("ChessPiece"))
                {
                    print("Clicked");
                }
                else
                {
                    print("Click Off");
                }
            }
            else
            {
                Debug.Log("Ray hit nothing");
            }
        }
    }
}