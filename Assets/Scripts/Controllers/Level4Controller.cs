using System;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Controla o n�vel 4.
 * O n�vel consiste em trocar de bomba de jogador para jogador,
 * at� que o tempo acabe e quem tem a bomba perde.
 * O n�vel � constituido por v�rias rondas.
*/
public class Level4Controller : MonoBehaviour
{
    // vari�veis para guardar os jogadores do n�vel
    public GameObject player1;
    public GameObject player2;
    int playerIDWithBomb = -1;

    // refer�ncias para a bomba
    public GameObject bombPrefab;
    public GameObject bomb;
    public BombController bombController;

    // para saber se os jogadores colidiram
    public bool collisionOccurred = false;


    void Start()
    {
        // previne que o Random n�o fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        int randomID = GenerateFirstPlayerToPlay();
        playerIDWithBomb = randomID;

        // exibir objetos na cena
        SpawnPlayer1();
        SpawnPlayer2();

        SpawnBomb();
        AssignBomb();
    }

    void Update()
    {

    }

    void SpawnPlayer1()
    {
        player1 = Instantiate(GameController.GetInstance().player1Prefab,
                              GameController.GetInstance().player1Prefab.transform.position,
                              GameController.GetInstance().player1Prefab.transform.rotation);
    }

    void SpawnPlayer2()
    {
        player2 = Instantiate(GameController.GetInstance().player2Prefab,
                              GameController.GetInstance().player2Prefab.transform.position,
                              GameController.GetInstance().player2Prefab.transform.rotation);
    }

    int GenerateFirstPlayerToPlay()
    {
        return Random.Range(1, 3);
    }

    public GameObject GetPlayerWithBomb()
    {
        if (playerIDWithBomb == 1)
        {
            return player2;
        }
        else
        {
            return player1;
        }
    }

    public void ChangePlayerTurn()
    {
        if (playerIDWithBomb == 1)
        {
            playerIDWithBomb = 2;
        }
        else if (playerIDWithBomb == 2)
        {
            playerIDWithBomb = 1;
        }
    }

    public void SpawnBomb()
    {
        bomb = Instantiate(bombPrefab, bombPrefab.transform.position, Quaternion.identity);
    }

    public void AssignBomb()
    {
        bombController = bomb.GetComponent<BombController>();
        bombController.SetPlayer(this.GetPlayerWithBomb());
        bombController.SetPlayerAsParent(this.GetPlayerWithBomb());
        bombController.SetLocalPosition(new Vector3(0.042f, 0.39f, 0.352f));
        bombController.SetLocalRotation(Quaternion.Euler(270f, 0f, 0f));
        bombController.SetLocalScale(new Vector3(85f, 85f, 85f));
    }

    void SpawnPowerUp()
    {
        // TODO
    }
}