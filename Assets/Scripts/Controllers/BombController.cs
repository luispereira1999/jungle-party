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
        FollowPlayer();
    }

    public void SpawnBomb()
    {
        // TODO

        // Exemplo de spawnar objeto:
        // GameObject newObject = Instantiate(bomb, transform.position + transform.forward * spawnDistance, transform.rotation);
        // newObject.SetActive(true);
    }

    public void FollowPlayer()
    {
        Vector3 playerPosition = currentPlayer.transform.position;
        Vector3 bombPosition = playerPosition + currentPlayer.transform.forward * distanceAhead;
        transform.position = bombPosition;
    }
}