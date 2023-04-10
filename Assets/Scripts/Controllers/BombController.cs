using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject currentPlayer;
    public float distanceAhead = 1f;

    void Start()
    {

    }

    void Update()
    {
        Vector3 playerPos = currentPlayer.transform.position;
        //Debug.Log(playerPos.x);
        Vector3 bombPos = playerPos + currentPlayer.transform.forward * distanceAhead;
        transform.position = bombPos;
    }

    public void SpawnBomb()
    {
        // TODO

        // Exemplo de spawnar objeto:
        // GameObject newObject = Instantiate(bomb, transform.position + transform.forward * spawnDistance, transform.rotation);
        // newObject.SetActive(true);
    }
}