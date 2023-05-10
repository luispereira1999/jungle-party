using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/*
 * Controla o n�vel 1.
 * O n�vel consiste em uma partida de futebol com v�rias rondas.
*/
public class Level1Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _gameController;

    // vari�veis sobre os jogadores
    private List<LevelPlayerModel> _levelPlayers = new();
    private int _playerIDWithBomb = -1;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para os objetos do n�vel - bola
    [SerializeField] private GameObject _ballPrefab;
    private GameObject _ballObject;

    // para os objetos do n�vel - balizas
    [SerializeField] private GameObject _goalPrefab1;
    [SerializeField] private GameObject _goalPrefab2;
    private GameObject _goalObject1;
    private GameObject _goalObject2;

    // para os objetos do n�vel - power ups
    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;

    // para definir a a��o dos jogadores neste n�vel
    private KickAction _kickAction;

    // refer�ncia do controlador do rel�gio
    private TimerController _timerController;

    // refer�ncia do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // refer�ncia do controlador da pontua��o
    [SerializeField] private ScoreController _scoreController;

    // para detetar que os objetos est�o congelados quando a ronda acaba
    private bool _freezeObjects = false;

    // para os componentes da UI - painel de introdu��o, bot�o de pause e painel do fim de n�vel
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _finishedLevelPanel;
    [SerializeField] private GameObject _finishedLevelDescription;


    /* PROPRIEDADES P�BLICAS */

    public bool CollisionOccurred
    {
        get { return _collisionOccurred; }
        set { _collisionOccurred = value; }
    }


    /* M�TODOS DO MONOBEHAVIOUR */

    void Start()
    {
        _gameController = GameController.Instance;

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _gameController.GamePlayers = new();
        _gameController.InitiateGame();

        // armazenar dados de cada jogador neste n�vel,
        // sabendo que um jogo tem v�rios n�veis e j� existem dados que passam de n�vel para n�vel, como a pontua��o
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
        // se o tempo da ronda ainda n�o acabou
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

            // se estiver na �ltima ronda - mostrar o painel do fim de n�vel
            if (_roundController.IsLastRound())
            {
                string finishedLevelText = "";
                foreach (LevelPlayerModel levelPlayer in _levelPlayers)
                {
                    finishedLevelText += "Jogador " + levelPlayer.ID + ": " + levelPlayer.LevelScore + "\n";
                }

                _finishedLevelPanel.SetActive(true);
                _finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;
            }
            // sen�o iniciar outra ronda
            else
            {
                _roundController.NextRound();
                _roundController.DisplayNextRoundIntro();
                _roundController.DisplayCurrentRound();

                Invoke(nameof(RestartRound), freezingTime);
            }
        }
    }


    /* M�TODOS DO LEVEL4CONTROLLER */

    /*
     * � executado ao clicar no bot�o de iniciar, no painel de introdu��o do n�vel.
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
            LevelPlayerModel levelPlayer = new(gamePlayer.ID, 0, gamePlayer.Prefab.transform.position, gamePlayer.Prefab.transform.rotation);
            _levelPlayers.Add(levelPlayer);
        }
    }

    int GenerateFirstPlayerWithBomb()
    {
        // previne que o Random n�o fique viciado
        Random.InitState(DateTime.Now.Millisecond);

        return Random.Range(1, 3);
    }

    void DisplayObjectInScene()
    {
        SpawnPlayers();
        AddActionToPlayers();
    }

    void SpawnPlayers()
    {
        _levelPlayers[0].Object = Instantiate(_gameController.GamePlayers[0].Prefab);
        _levelPlayers[1].Object = Instantiate(_gameController.GamePlayers[1].Prefab);
    }

    /*
     * Adiciona o script da a��o a cada um dos objetos dos jogadores, para definir essa a��o ao personagem.
    */
    void AddActionToPlayers()
    {
        _kickAction = _levelPlayers[0].Object.AddComponent<KickAction>();
        _levelPlayers[0].Object.GetComponent<PlayerController>().SetAction(_kickAction, this);

        _kickAction = _levelPlayers[1].Object.AddComponent<KickAction>();
        _levelPlayers[1].Object.GetComponent<PlayerController>().SetAction(_kickAction, this);
    }

    void FreezePlayers(float freezingTime)
    {
        _levelPlayers[0].Object.GetComponent<PlayerController>().Freeze(freezingTime);
        _levelPlayers[1].Object.GetComponent<PlayerController>().Freeze(freezingTime);
    }

    /*
     * Atribui os pontos do vencedor e atualiza no ecr�.
    */
    void UpdateWinnerScore()
    {
        int winnerID = GetWinnerID();

        _levelPlayers[winnerID - 1].LevelScore += _scoreController.AddScore();
        _scoreController.DisplayScoreObjectText(winnerID, _levelPlayers[winnerID - 1].LevelScore);
    }

    int GetWinnerID()
    {
        return _playerIDWithBomb == 1 ? 2 : 1;
    }

    /*
     * � executado ap�s o intervalo de espera para iniciar outra ronda.
     * Respons�vel por inicializar novamente os componentes necess�rios para que a ronda comece.
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
        _levelPlayers[0].Object.transform.position = _levelPlayers[0].InitialPosition;
        _levelPlayers[0].Object.transform.rotation = _levelPlayers[0].InitialRotation;

        _levelPlayers[1].Object.transform.position = _levelPlayers[1].InitialPosition;
        _levelPlayers[1].Object.transform.rotation = _levelPlayers[1].InitialRotation;
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
            return _levelPlayers[0].Object;
        }
        else
        {
            return _levelPlayers[1].Object;
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
     * � executado quando � clicado o bot�o de pr�ximo n�vel, no painel de fim de n�vel.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }
}