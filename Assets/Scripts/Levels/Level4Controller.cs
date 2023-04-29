using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 * Controla o nível 4.
 * O nível consiste em trocar de bomba de jogador para jogador,
 * até que o tempo acabe e quem tem a bomba perde.
 * O nível é constituido por várias rondas.
*/
public class Level4Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _game;

    // variáveis para guardar os jogadores do nível
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    [SerializeField] int _rounds;
    [SerializeField] int _points;
    private int _currentRound;

    // referências para a bomba
    [SerializeField] private GameObject _bombPrefab;

    private GameObject _bombObject;
    private BombController _bombController;
    private TimerController _timer;
    private Text _roundsComponent;
    const string _roundsText = "Round {0}";

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a ação dos jogadores neste nível
    private ThrowController _throwController;

    private List<GameObject> _powerUps;

    private TimerController timer;

    [SerializeField] GameObject _powerUp;


    /* PROPRIEDADES PÚBLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* MÉTODOS */

    void Start() {
        _game = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
        _game.Players = new List<PlayerModel>();
        _game.InitiateGame();

        _powerUps = new List<GameObject>();

        _timer = TimerController.Instance;

        _currentRound = 1;
        Time.timeScale = 0f;

        _roundsComponent = GameObject.Find("Rounds").GetComponent<Text>();
        _roundsComponent.text = String.Format(_roundsText, _currentRound);

        // previne que o Random não fique viciado
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
    }

    void Update()
    {
        if (_timer.hasFinished()) {

            _timer.Restart();
            _currentRound++;

            _roundsComponent.text = String.Format(_roundsText, _currentRound);

            Time.timeScale = 0f;

            Invoke("RestartRound", 5f);

            //StartCoroutine(MyCoroutine());
        }
    }

    public void Init()
    {
        Time.timeScale = 1f;

        Destroy(GameObject.Find("IntroPanel"));

        InvokeRepeating("CreatePowerUp", 5f, 10f);
    }

    void RestartRound()
    {
        Time.timeScale = 1f;

        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");

        // Loop through each object and destroy it
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }

    IEnumerator MyCoroutine()
    {
        

        yield return new WaitForSeconds(3);

        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");

        // Loop through each object and destroy it
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        

    }

    void CreatePowerUp()
    {
        System.Random rnd = new System.Random();
        int xValue = rnd.Next(72, 440);
        int zValue = rnd.Next(62, 430);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
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