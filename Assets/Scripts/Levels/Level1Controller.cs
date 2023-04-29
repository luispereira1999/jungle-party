using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Controla o n�vel 1.
 * O n�vel consiste em uma partida de futebol com v�rias rondas.
*/
public class Level1Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _game;

    // vari�veis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    // refer�ncias para a bola
    [SerializeField] GameObject _ballPrefab;
    private GameObject _ballObject;

    // refer�ncias para a baliza
    [SerializeField] private GameObject _goalPrefab;
    private GameObject _goalObject;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a a��o dos jogadores neste n�vel
    private KickController _kickController;


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

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _game.Players = new List<PlayerModel>();
        _game.InitiateGame();

        int randomID = GenerateFirstPlayerToPlay();
        _playerIDWithBomb = randomID;

        // exibir objetos na cena
        SpawnPlayer1();
        SpawnPlayer2();

        AddActionToPlayer1();
        AddActionToPlayer2();
    }

    void Update()
    {

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
        _kickController = _player1Object.AddComponent<KickController>();
        _player1Object.GetComponent<PlayerController>().SetAction(_kickController, this);
    }

    void AddActionToPlayer2()
    {
        _kickController = _player2Object.AddComponent<KickController>();
        _player2Object.GetComponent<PlayerController>().SetAction(_kickController, this);
    }

    int GenerateFirstPlayerToPlay()
    {
        // previne que o Random n�o fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        return Random.Range(1, 3);
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

    void SpawnPowerUp()
    {
        // TODO
    }
}