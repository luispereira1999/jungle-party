using UnityEngine;

/*
 * Controla o nível 4.
*/
public class Level4Controller : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    public GameObject bombPrefab;
    private BombController bombController;
    //public float spawnDistance = 2f;
    public float distanceAhead = 1f;

    void Awake()
    {

    }

    void Start()
    {
        player1 = Instantiate(GameController.GetInstance().player1Prefab,
                              GameController.GetInstance().player1Prefab.transform.position,
                              GameController.GetInstance().player1Prefab.transform.rotation);

        player2 = Instantiate(GameController.GetInstance().player2Prefab,
                              GameController.GetInstance().player2Prefab.transform.position,
                              GameController.GetInstance().player2Prefab.transform.rotation);

        bombController = bombPrefab.GetComponent<BombController>();
        bombController.currentPlayer = player1;
        Instantiate(bombPrefab, transform.position, transform.rotation);
    }

    void Update()
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