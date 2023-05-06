using System;
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
    private GameController _gameController;

    // variáveis sobre os jogadores
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
    private ThrowAction _throwAction;

    // referência do controlador do relógio
    private TimerController _timerController;

    // referência do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // referência do controlador da pontuação
    [SerializeField] private ScoreController _scoreController;

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
        _gameController.GamePlayers = new();
        _gameController.InitiateGame();

        // armazenar dados de cada jogador neste nível,
        // sabendo que um jogo tem vários níveis e já existem dados que passam de nível para nível, como a pontuação
        CreatePlayersDataForLevel();

        _timerController = TimerController.Instance;
        TimerController.Freeze();

        _roundController.DisplayCurrentRound();

        int randomID = GenerateFirstPlayerWithBomb();
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

        // se a ronda acaba - congelar objetos, cancelar spawn de power ups e atribuir pontos
        if (!_freezeObjects)
        {
            _freezeObjects = true;
            float freezingTime = 5f;
            FreezePlayers(freezingTime);

            CancelInvoke(nameof(SpawnPowerUp));

            UpdateWinnerScore();

            // se estiver na última ronda - mostrar o painel do fim de nível
            if (_roundController.IsLastRound())
            {
                string finishedLevelText = "";
                foreach (LevelPlayerModel levelPlayer in levelPlayers)
                {
                    finishedLevelText += "Jogador " + levelPlayer.ID + ": " + levelPlayer.LevelScore + "\n";
                }

                _finishedLevelPanel.SetActive(true);
                _finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;
            }
            // senão iniciar outra ronda
            else
            {
                _roundController.NextRound();
                _roundController.DisplayNextRoundIntro();
                _roundController.DisplayCurrentRound();

                Invoke(nameof(RestartRound), freezingTime);
            }
        }
    }


    /* MÉTODOS DO LEVEL4CONTROLLER */

    /*
     * É executado ao clicar no botão de iniciar, no painel de introdução do nível.
     * Permite que os jogadores comecem de facto a jogar.
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

    void SpawnPowerUp()
    {
        System.Random rnd = new();
        int xValue = rnd.Next(42, 58);
        int zValue = rnd.Next(71, 84);

        Instantiate(_powerUp, new Vector3(xValue, _powerUp.transform.position.y, zValue), Quaternion.identity);
    }

    void CreatePlayersDataForLevel()
    {
        foreach (GamePlayerModel gamePlayer in _gameController.GamePlayers)
        {
            LevelPlayerModel levelPlayer = new(gamePlayer.ID, 0);
            levelPlayers.Add(levelPlayer);
        }
    }

    int GenerateFirstPlayerWithBomb()
    {
        // previne que o Random não fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        return Random.Range(1, 3);
    }

    void DisplayObjectInScene()
    {
        SpawnPlayers();
        AddActionToPlayers();

        SpawnBomb();
        AssignBomb();
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

    void SpawnPlayers()
    {
        _player1Object = Instantiate(_gameController.GamePlayers[0].Prefab);
        levelPlayers[0].InitialPosition = _player1Object.transform.position;
        levelPlayers[0].InitialRotation = _player1Object.transform.rotation;

        _player2Object = Instantiate(_gameController.GamePlayers[1].Prefab);
        levelPlayers[1].InitialPosition = _player2Object.transform.position;
        levelPlayers[1].InitialRotation = _player2Object.transform.rotation;
    }

    /*
     * Adiciona o script da ação a cada um dos objetos dos jogadores, para definir essa ação ao personagem.
    */
    void AddActionToPlayers()
    {
        _throwAction = _player1Object.AddComponent<ThrowAction>();
        _player1Object.GetComponent<PlayerController>().SetAction(_throwAction, this);

        _throwAction = _player2Object.AddComponent<ThrowAction>();
        _player2Object.GetComponent<PlayerController>().SetAction(_throwAction, this);
    }

    void FreezePlayers(float freezingTime)
    {
        _player1Object.GetComponent<PlayerController>().Freeze(freezingTime);
        _player2Object.GetComponent<PlayerController>().Freeze(freezingTime);
    }

    /*
     * Atribui os pontos do vencedor e atualiza no ecrã.
    */
    void UpdateWinnerScore()
    {
        int winnerID = GetWinnerID();

        levelPlayers[winnerID - 1].LevelScore += _scoreController.PointsPerRound;
        _scoreController.DisplayScoreObjectText(winnerID, levelPlayers[winnerID - 1].LevelScore);
    }

    int GetWinnerID()
    {
        return _playerIDWithBomb == 1 ? 2 : 1;
    }

    /*
     * É executado após o intervalo de espera para iniciar outra ronda.
     * Responsável por inicializar novamente os componentes necessários para que a ronda comece.
    */
    void RestartRound()
    {
        _freezeObjects = false;

        _timerController.SetInitialTime();

        _roundController.DisableNextRoundIntro();

        SetInitialPosition();

        DestroyAllPowerUps();
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

    public GameObject GetPlayerWithBomb()
    {
        if (_playerIDWithBomb == 1)
        {
            return _player1Object;
        }
        else
        {
            return _player2Object;
        }
    }

    public void ChangePlayerTurn()
    {
        if (_playerIDWithBomb == 1)
        {
            _playerIDWithBomb = 2;
        }
        else
        {
            _playerIDWithBomb = 1;
        }
    }

    /*
     * É executado quando é clicado o botão de próximo nível, no painel de fim de nível.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(levelPlayers[0].LevelScore, levelPlayers[1].LevelScore);
    }
}