using UnityEngine;
using UnityEngine.EventSystems;

public class SelectChess : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            if (hits.Length > 0)
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
