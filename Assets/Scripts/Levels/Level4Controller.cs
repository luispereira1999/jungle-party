using System;
using System.Collections;
using System.Collections.Generic;
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
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _game;

    private int i = 0;

    // vari�veis para guardar os jogadores do n�vel
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    // refer�ncias para a bomba
    [SerializeField] private GameObject _bombPrefab;
    private GameObject _bombObject;
    private BombController _bombController;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a a��o dos jogadores neste n�vel
    private ThrowController _throwController;

    private List<GameObject> powerUps;

    [SerializeField] GameObject powerUp;


    /* PROPRIEDADES P�BLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* M�TODOS */

    void Start()
    {
        _game = GameController.Instance;

        powerUps = new List<GameObject>();
        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _game.Players = new List<PlayerModel>();
        _game.InitiateGame();

        // previne que o Random n�o fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        int randomID = GenerateFirstPlayerToPlay();
        _playerIDWithBomb = randomID;

        // exibir objetos na cena
        SpawnPlayer1();
        SpawnPlayer2();

        AddActionToPlayer1();
        AddActionToPlayer2();

        SpawnBomb();
        AssignBomb();

        InvokeRepeating("createPowerUp", 5f, 10f);
    }

    void Update()
    {

    }


    void createPowerUp()
    {
        System.Random rnd = new System.Random();
        int xValue = rnd.Next(72, 440);
        int zValue = rnd.Next(62, 430);

        GameObject instantiatedObject = Instantiate(powerUp, new Vector3(xValue, 55, zValue), Quaternion.identity);
        instantiatedObject.transform.localScale = new Vector3(4f, 4f, 4f);
        instantiatedObject.AddComponent<BoxCollider>();
       instantiatedObject.tag = "PowerUp";
        
        //instantiatedObject.tag = isPowerUp ? "PowerUp" : "PowerDown";


        i++;
    }

    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_game.Players[0].prefab);
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_game.Players[1].prefab);
    }

    /*
     * Adiciona o script da a��o ao objeto do jogador, para definir essa a��o ao personagem.
    */
    void AddActionToPlayer1()
    {
        _throwController = _player1Object.AddComponent<ThrowController>();
        _player1Object.GetComponent<PlayerController>().SetAction(_throwController, this);
    }

    void AddActionToPlayer2()
    {
        _throwController = _player2Object.AddComponent<ThrowController>();
        _player2Object.GetComponent<PlayerController>().SetAction(_throwController, this);
    }

    int GenerateFirstPlayerToPlay()
    {
        return Random.Range(1, 3);
    }

    public GameObject GetPlayerWithBomb()
    {
        if (_playerIDWithBomb == 1)
        {
            return _player2Object;
        }
        else
        {
            return _player1Object;
        }
    }

    public void ChangePlayerTurn()
    {
        if (_playerIDWithBomb == 1)
        {
            _playerIDWithBomb = 2;
        }
        else if (_playerIDWithBomb == 2)
        {
            _playerIDWithBomb = 1;
        }
    }

    void SpawnBomb()
    {
        _bombObject = Instantiate(_bombPrefab, _bombPrefab.transform.position, Quaternion.identity);
    }

    public void AssignBomb()
    {
        _bombController = _bombObject.GetComponent<BombController>();
        _bombController.SetPlayer(this.GetPlayerWithBomb());
        _bombController.SetPlayerAsParent(this.GetPlayerWithBomb());
        _bombController.SetLocalPosition(new Vector3(0.042f, 0.39f, 0.352f));
        _bombController.SetLocalRotation(Quaternion.Euler(270f, 0f, 0f));
        _bombController.SetLocalScale(new Vector3(85f, 85f, 85f));
    }

    void SpawnPowerUp()
    {
        // TODO
    }
}