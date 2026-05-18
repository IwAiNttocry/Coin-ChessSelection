using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine .EventSystems;
using UnityEngine.UI;


public class ChessBehaviour : MonoBehaviour
{
    private Transform selection;
    private RaycastHit raycasthit;    
    Vector2 mousePosition;  

    void Update()
    {
        mousePosition = Input.mousePosition;
        print(mousePosition);

    }
}