using UnityEngine;

/*
 * Controla o nível 4.
 * O nível consiste em trocar de bomba de jogador para jogador,
 * até que o tempo acabe e quem tem a bomba perde.
 * O nível é constituido por várias rondas.
*/
public class Level4Controller : MonoBehaviour
{
    private GameObject player1;
    private GameObject player2;
    public GameObject bombPrefab;
    private GameObject bomb;
    private BombController bombController;

    void Start()
    {
        /*
         * Jogadores
        */
        player1 = Instantiate(GameController.GetInstance().player1Prefab,
                              GameController.GetInstance().player1Prefab.transform.position,
                              GameController.GetInstance().player1Prefab.transform.rotation);

        player2 = Instantiate(GameController.GetInstance().player2Prefab,
                              GameController.GetInstance().player2Prefab.transform.position,
                              GameController.GetInstance().player2Prefab.transform.rotation);

        /*
         * Bomba
        */
        bomb = Instantiate(bombPrefab, player1.transform.position, Quaternion.identity);

        bombController = bomb.GetComponent<BombController>();
        bombController.SetPlayer(player1);
        bombController.SetPosition(player1.transform.position + new Vector3(0.4f, 0.7f, -0.5f));
        bombController.SetRotation(Quaternion.Euler(-90f, 0f, 0f));
        bombController.SetScale(new Vector3(140f, 140f, 140f));
        bombController.SetPlayerAsParent(player1);
    }

    void Update()
    {

    }

    public void SpawnBomb()
    {
        // TODO
    }

    public void SpawnPowerUp()
    {
        // TODO
    }
}