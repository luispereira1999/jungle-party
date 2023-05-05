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

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para os objetos do nível - bomba
    [SerializeField] private GameObject _bombPrefab;
    private GameObject _bombObject;
    private BombController _bombController;

    // para os objetos do nível - power ups
    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;

    // para definir a ação dos jogadores neste nível
    private ThrowController _throwController;

    // referência do controlador do relógio
    private TimerController _timerController;

    // referência do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // para detetar que os objetos estão congelados quando a ronda acaba
    private bool _freezeObjects = false;

    // para o modelo de dados do jogador referente ao nível
    private List<LevelPlayerModel> levelPlayers = new();

    // para os componentes da UI - painel de introdução, botão de pause e painel do fim de nível
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _finishedLevelPanel;
    [SerializeField] private GameObject _finishedLevelDescription;


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

    void Update()
    {
        // se o tempo da ronda ainda não acabou
        if (!_timerController.HasFinished())
        {
            return;
        }

        // se a ronda acabou - congelar objetos, cancelar spawn de power ups e atribuir pontos
        if (!_freezeObjects)
        {
            _freezeObjects = true;
            float freezingTime = 5f;
            FreezePlayers(freezingTime);

            CancelInvoke(nameof(SpawnPowerUp));

            SetWinnerPoints();

            // se estiver na última ronda - mostrar o painel do fim de nível
            if (_roundController.IsLastRound())
            {
                string textPoints = "";
                foreach (LevelPlayerModel levelPlayer in levelPlayers)
                {
                    textPoints += "Jogador " + levelPlayer.Id + ": " + levelPlayer.LevelScore + "\n";
                }

                _finishedLevelPanel.SetActive(true);
                _finishedLevelDescription.GetComponent<Text>().text = textPoints;
            }
            // senão iniciar outra ronda
            else
            {
                _roundController.NextRound();
                _roundController.DisplayNextRoundIntro();
                Invoke(nameof(RestartRound), freezingTime);
            }
        }
    }


    /* MÉTODOS DO LEVEL4CONTROLLER */

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

    void RestartRound()
    {
        _timerController.SetInitialTime();
        _freezeObjects = false;

        _roundController.DisableNextRoundIntro();

        SetInitialPosition();

        DestroyAllPowerUps();

        _roundController.DisplayCurrentRound();
    }

    void SetWinnerPoints()
    {
        int winnerId = _playerIDWithBomb == 1 ? 2 : 1;

        LevelPlayerModel levelPlayer = levelPlayers[winnerId - 1];

        levelPlayer.LevelScore += _roundController.PointsPerRound;

        string scoreText = winnerId == 1 ? "ScoreP2Text" : "ScoreP1Text";

        GameObject[] scoreTextComp = GameObject.FindGameObjectsWithTag(scoreText);
        scoreTextComp[0].GetComponent<TextMeshProUGUI>().text = levelPlayer.LevelScore.ToString();
    }

    void FreezePlayers(float freezingTime)
    {
        _player1Object.GetComponent<PlayerController>().Freeze(freezingTime);
        _player2Object.GetComponent<PlayerController>().Freeze(freezingTime);
    }

    void SetInitialPosition()
    {
        _player1Object.transform.position = levelPlayers[0].InitialPosition;
        _player2Object.transform.position = levelPlayers[1].InitialPosition;

        _player1Object.transform.rotation = levelPlayers[0].InitialRotation;
        _player2Object.transform.rotation = levelPlayers[1].InitialRotation;
    }

    void DestroyAllPowerUps()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }

    void DisplayObjectInScene()
    {
        SpawnPlayers();
        AddActionToPlayers();

        SpawnBomb();
        AssignBomb();
    }

    void SpawnPlayers()
    {
        _player1Object = Instantiate(_gameController.Players[0].prefab);
        levelPlayers[0].InitialPosition = _player1Object.transform.position;
        levelPlayers[0].InitialRotation = _player1Object.transform.rotation;

        _player2Object = Instantiate(_gameController.Players[1].prefab);
        levelPlayers[1].InitialPosition = _player2Object.transform.position;
        levelPlayers[1].InitialRotation = _player2Object.transform.rotation;
    }

    /*
     * Adiciona o script da ação a cada um dos objetos dos jogadores, para definir essa ação ao personagem.
    */
    void AddActionToPlayers()
    {
        _throwController = _player1Object.AddComponent<ThrowController>();
        _player1Object.GetComponent<PlayerController>().SetAction(_throwController, this);

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