using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

    // para os objetos do nível - bola, balizas e power ups
    [SerializeField] private GameObject _ballPrefab;
    private GameObject _ballObject;

    [SerializeField] private GameObject _goalPrefab1;
    [SerializeField] private GameObject _goalPrefab2;
    private GameObject _goalObject1;
    private GameObject _goalObject2;

    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;
    private bool _freezeComponents = false;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    private List<LevelPlayerModel> levelPlayers;

    // para definir a ação dos jogadores neste nível
    private KickController _kickController;

    // para o tempo, rondas e pontuação
    private TimerController _timer;

    [SerializeField] private int _rounds;
    [SerializeField] private int _roundPoints;
    [SerializeField] private Text _roundsComponent;
    private int _currentRound = 0;

    // para a popup do fim de nível
    [SerializeField] private GameObject _popUpWinner;

    // para os componentes da UI - painel de introdução, botão de pause e 
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _nextRoundObject;


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
        _game.Players = new List<GamePlayerModel>();
        _game.InitiateGame();

        levelPlayers = new List<LevelPlayerModel>();
        _timer = TimerController.Instance;

        foreach (GamePlayerModel player in _game.Players)
        {
            LevelPlayerModel levelPlayer = new();

            levelPlayer.Id = player.id;
            levelPlayers.Add(levelPlayer);
        }

        TimerController.Freeze();

        _roundsComponent.text = _currentRound.ToString();

        int randomID = GenerateFirstPlayerToPlay();
        _playerIDWithBomb = randomID;

        // exibir objetos na cena
        CreateObjectInScene();
    }

    /*
     * É executado ao clicar no botão de iniciar, no painel de introdução do nível.
    */
    public void InitAfterIntro()
    {
        TimerController.Unfreeze();

        _currentRound++;
        _roundsComponent.text = _currentRound.ToString();

        _buttonPause.SetActive(true);

        Destroy(_introPanel);

        InvokeRepeating(nameof(SpawnPowerUp), 5f, 10f);
    }

    void CreateObjectInScene()
    {
        SpawnPlayer1();
        SpawnPlayer2();

        AddActionToPlayer1();
        AddActionToPlayer2();
    }

    void Update()
    {
        if (!_timer.HasFinished())
        {
            return;
        }

        if (_rounds == _currentRound && !_freezeComponents)
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

            _currentRound++;

            _nextRoundObject.SetActive(true);
            _nextRoundObject.GetComponent<Text>().text = "Ronda: " + _currentRound.ToString();
            Invoke(nameof(RestartRound), freezeTime);
        }
    }

    public void Init()
    {
        Time.timeScale = 1f;

        Destroy(GameObject.Find("IntroPanel"));

        InvokeRepeating(nameof(SpawnPowerUp), 5f, 10f);
    }

    void RestartRound()
    {
        _nextRoundObject.SetActive(false);

        _timer.SetInitialTime();
        _freezeComponents = false;

        SetInitialPosition();

        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");

        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }

        _roundsComponent.text = _currentRound.ToString();
    }

    void CreatePowerUp()
    {
        System.Random rnd = new();
        int xValue = rnd.Next(72, 440);
        int zValue = rnd.Next(62, 430);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }

    void SetLevelPoints()
    {
        int winnerId = _playerIDWithBomb == 1 ? 2 : 1;

        LevelPlayerModel levelPlayer = levelPlayers[winnerId - 1];

        levelPlayer.LevelScore += _roundPoints;

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

    void SpawnPlayer1()
    {
        _player1Object = Instantiate(_game.Players[0].prefab);
        levelPlayers[0].InitialPosition = _player1Object.transform.position;
        levelPlayers[0].InitialRotation = _player1Object.transform.rotation;
    }

    void SpawnPlayer2()
    {
        _player2Object = Instantiate(_game.Players[1].prefab);
        levelPlayers[1].InitialPosition = _player2Object.transform.position;
        levelPlayers[1].InitialRotation = _player2Object.transform.rotation;
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
        System.Random rnd = new();
        int xValue = rnd.Next(42, 58);
        int zValue = rnd.Next(71, 84);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }
}