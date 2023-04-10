using UnityEngine;

public class Level4Controller : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public GameObject bombPrefab;
    private BombController bombController;
    public float distanceAhead = 1f;
    int a;

    void Start()
    {
        GameObject a = FindObjectOfType<GameController>().player1;
    }

    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    public void SpawnBomb()
    {
        // TODO

        // Exemplo de spawnar objeto:
        // GameObject newObject = Instantiate(bomb, transform.position + transform.forward * spawnDistance, transform.rotation);
        // newObject.SetActive(true);
    }

    public void SpawnPowerUp()
    {
        // TODO
    }
}