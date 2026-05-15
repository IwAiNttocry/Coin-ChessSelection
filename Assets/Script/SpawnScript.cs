using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public GameObject chessPiecePrefab;
    public Transform[] spawnPoints; // Drag multiple positions here

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(chessPiecePrefab, point.position, point.rotation);
        }
    }
}
