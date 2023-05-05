using System;
using System.Collections.Generic;
using TMPro;
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
    private GameController _gameController;

    // variáveis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;
    private int _playerIDWithBomb = -1;

    // para os objetos do nível - bomba e power ups
    [SerializeField] private GameObject _bombPrefab;
    private GameObject _bombObject;
    private BombController _bombController;

    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;
    private bool _freezeComponents = false;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    private List<LevelPlayerModel> levelPlayers;

    // para definir a ação dos jogadores neste nível
    private ThrowController _throwController;

    // para controlar o relógio
    private TimerController _timerController;

    // para controlar as rondas
    [SerializeField] private RoundController _roundController;

    // para a popup do fim de nível
    [SerializeField] private GameObject _popUpWinner;

    // para os componentes da UI - painel de introdução, botão de pause e 
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;


    /* PROPRIEDADES PÚBLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* MÉTODOS DO MONOBEHAVIOUR */

    void Start()
    {
        _gameController = GameController.Instance;

        // TEST: usar isto enquanto é testado apenas o nível atual (sem iniciar pelo menu)
        _gameController.Players = new List<GamePlayerModel>();
        _gameController.InitiateGame();

        levelPlayers = new List<LevelPlayerModel>();

        foreach (GamePlayerModel player in _gameController.Players)
        {
            LevelPlayerModel levelPlayer = new();

            levelPlayer.Id = player.id;
            levelPlayers.Add(levelPlayer);
        }

        _timerController = TimerController.Instance;
        TimerController.Freeze();

        _roundController.DisplayCurrentRound();

        int randomID = GenerateFirstPlayerToPlay();
        _playerIDWithBomb = randomID;

        DisplayObjectInScene();
    }

    /*
     * É executado ao clicar no botão de iniciar, no painel de introdução do nível.
    */
    public void InitAfterIntro()
    {
        TimerController.Unfreeze();

        _roundController.NextRound();
        _roundController.DisplayCurrentRound();

        _buttonPause.SetActive(true);

        Destroy(_introPanel);

        InvokeRepeating(nameof(SpawnPowerUp), 5f, 10f);
    }

    void Update()
    {
        if (!_timerController.HasFinished())
        {
            return;
        }

        if (_roundController.NumberOfRounds == _roundController.CurrentRound && !_freezeComponents)
        {
            _freezeComponents = true;
            _popUpWinner.SetActive(true);

            CancelInvoke(nameof(SpawnPowerUp));

            SetLevelPoints();

            GameObject[] popUpText = GameObject.FindGameObjectsWithTag("PopUpText");

            string textPoints = "";

            foreach (LevelPlayerModel levelPlayer in levelPlayers)
            {
                textPoints += "Jogador " + levelPlayer.Id + ": " + levelPlayer.LevelScore + "\n";
            }

            popUpText[0].GetComponent<Text>().text = textPoints;

            SetInitialPosition();
        }
        else if (!_freezeComponents)
        {
            float freezeTime = 5f;
            _freezeComponents = true;

            SetLevelPoints();

            _player1Object.GetComponent<PlayerController>().Freeze(freezeTime);
            _player2Object.GetComponent<PlayerController>().Freeze(freezeTime);

            _roundController.NextRound();

            _roundController.DisplayNextRoundIntro();

            Invoke(nameof(RestartRound), freezeTime);
        }
    }


    /* MÉTODOS DO LEVEL4CONTROLLER */

    public void Init()
    {
        Time.timeScale = 1f;

        Destroy(GameObject.Find("IntroPanel"));

        InvokeRepeating(nameof(SpawnPowerUp), 5f, 10f);
    }

    void RestartRound()
    {
        _timerController.SetInitialTime();
        _freezeComponents = false;

        _roundController.DisableNextRoundIntro();

        SetInitialPosition();

        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        _roundController.DisplayCurrentRound();
    }

    void SetLevelPoints()
    {
        int winnerId = _playerIDWithBomb == 1 ? 2 : 1;

        LevelPlayerModel levelPlayer = levelPlayers[winnerId - 1];

        levelPlayer.LevelScore += _roundController.PointsPerRound;

        string scoreText = winnerId == 1 ? "ScoreP2Text" : "ScoreP1Text";

        GameObject[] scoreTextComp = GameObject.FindGameObjectsWithTag(scoreText);
        scoreTextComp[0].GetComponent<TextMeshProUGUI>().text = levelPlayer.LevelScore.ToString();
    }

    void SetInitialPosition()
    {
        _player1Object.transform.position = levelPlayers[0].InitialPosition;
        _player2Object.transform.position = levelPlayers[1].InitialPosition;

        _player1Object.transform.rotation = levelPlayers[0].InitialRotation;
        _player2Object.transform.rotation = levelPlayers[1].InitialRotation;
    }

    void DisplayObjectInScene()
    {
        SpawnPlayer1();
        SpawnPlayer2();

        AddActionToPlayer1();
        AddActionToPlayer2();

        SpawnBomb();
        AssignBomb();
    }

    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_gameController.Players[0].prefab);
        levelPlayers[0].InitialPosition = _player1Object.transform.position;
        levelPlayers[0].InitialRotation = _player1Object.transform.rotation;
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_gameController.Players[1].prefab);
        levelPlayers[1].InitialPosition = _player2Object.transform.position;
        levelPlayers[1].InitialRotation = _player2Object.transform.rotation;
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
        // previne que o Random não fique viciado
        Random.InitState(DateTime.Now.Millisecond);

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
        System.Random rnd = new();
        int xValue = rnd.Next(42, 58);
        int zValue = rnd.Next(71, 84);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }
}