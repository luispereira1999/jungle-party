using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Controla o nível 1.
 * O nível consiste em uma partida de futebol com várias rondas.
*/
public class Level1Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _game;

    // variáveis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    // referências para a bola
    [SerializeField] GameObject _ballPrefab;
    private GameObject _ballObject;

    // referências para a baliza
    [SerializeField] private GameObject _goalPrefab;
    private GameObject _goalObject;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a ação dos jogadores neste nível
    private KickController _kickController;


    /* PROPRIEDADES PÚBLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* MÉTODOS */

    void Start()
    {
        _game = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
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
     * Adiciona o script da ação ao objeto do jogador, para definir essa ação ao personagem.
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
        // previne que o Random não fique viciado
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