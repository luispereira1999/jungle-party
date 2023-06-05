using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


/// <summary>
/// Controla o n�vel 1.
/// O n�vel consiste em uma partida de futebol com v�rias rondas.
/// </summary>
public class Level1Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _gameController;

    // vari�veis sobre os jogadores
    private List<LevelPlayerModel> _levelPlayers = new();

    // para os objetos do n�vel - bola
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private GameObject _ballObject;
    private BallController _ballController;

    // para os objetos do n�vel - balizas
    [SerializeField] private GameObject _goalObject1;
    [SerializeField] private GameObject _goalObject2;

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

    // para os componentes da UI - painel de introdu��o, bot�o de pause e painel do fim de n�vel
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _finishedLevelPanel;
    [SerializeField] private GameObject _finishedLevelDescription;


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
        _roundController.DisplayMaxRounds();

        DisplayObjectInScene();

        _ballController = _ballObject.GetComponent<BallController>();
    }

    void Update()
    {
        // quando est� no intervalo entre rondas, ou seja o tempo est� parado
        if (_timerController.IsOnPause())
        {
            return;
        }

        // se algu�m marcou golo
        // - congelar objetos, cancelar spawn de power ups, atribuir pontos e iniciar nova ronda
        if (_ballController.IsGoalScored())
        {
            float freezingTime = 5f;
            FreezePlayers(freezingTime);

            _timerController.Pause();

            CancelInvoke(nameof(SpawnPowerUp));

            LevelPlayerModel scorer = GetScorer();
            UpdateScore(scorer.ID);

            _roundController.NextRound();
            _roundController.DisplayNextRoundIntro();
            _roundController.DisplayCurrentRound();

            Invoke(nameof(RestartRound), freezingTime);
        }

        // se o tempo global acabou ou o n�mero m�ximo de golos foi atingido
        // - congelar objetos, cancelar spawn de power ups, atribuir pontos e mostrar o painel do fim de n�vel
        if (_timerController.HasFinished() || _roundController.IsLastRound())
        {
            // congela para sempre
            FreezePlayers(-1);

            _timerController.Pause();

            CancelInvoke(nameof(SpawnPowerUp));

            string finishedLevelText = "";
            foreach (LevelPlayerModel levelPlayer in _levelPlayers)
            {
                finishedLevelText += "Jogador " + levelPlayer.ID + ": " + levelPlayer.LevelScore + "\n";
            }

            _finishedLevelPanel.SetActive(true);
            _finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;

            _buttonPause.SetActive(false);
        }
    }


    /* M�TODOS DO LEVEL4CONTROLLER */

    /// <summary>
    /// � executado ao clicar no bot�o de iniciar, no painel de introdu��o do n�vel.
    /// Permite que os jogadores comecem de facto a jogar.
    /// </summary>
    public void InitAfterIntro()
    {
        TimerController.Unfreeze();
        _timerController.PlaySound();

        _roundController.NextRound();
        _roundController.DisplayCurrentRound();

        _buttonPause.SetActive(true);
        Destroy(_introPanel);

        InvokeRepeating(nameof(SpawnPowerUp), 10f, 10f);
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

    /// <summary>
    /// Adiciona o script da a��o a cada um dos objetos dos jogadores, para definir essa a��o ao personagem.
    /// </summary>
    void AddActionToPlayers()
    {
        _kickAction = _levelPlayers[0].Object.AddComponent<KickAction>();
        _levelPlayers[0].Object.GetComponent<PlayerController>().SetAction(_kickAction, this);

        _kickAction = _levelPlayers[1].Object.AddComponent<KickAction>();
        _levelPlayers[1].Object.GetComponent<PlayerController>().SetAction(_kickAction, this);
    }

    LevelPlayerModel GetScorer()
    {
        if (_ballController.Player1Scored)
        {
            // redefinir o jogador que marcou como falso,
            // para que quando inicia uma nova rodada, ainda nenhum jogador marcou
            _ballController.Player1Scored = false;
            _ballController.Player2Scored = false;

            return _levelPlayers[0];
        }
        else if (_ballController.Player2Scored)
        {
            _ballController.Player1Scored = false;
            _ballController.Player2Scored = false;

            return _levelPlayers[1];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Atribui os pontos do marcador e atualiza no ecr�.
    /// </summary>
    void UpdateScore(int scorerID)
    {
        _levelPlayers[scorerID - 1].LevelScore += _scoreController.AddScore();
        _scoreController.DisplayScoreObjectText(scorerID, _levelPlayers[scorerID - 1].LevelScore);
    }

    void FreezePlayers(float freezingTime)
    {
        _levelPlayers[0].Object.GetComponent<PlayerController>().Freeze(freezingTime);
        _levelPlayers[1].Object.GetComponent<PlayerController>().Freeze(freezingTime);
    }

    /// <summary>
    /// � executado ap�s o intervalo de espera para iniciar outra ronda.
    /// Respons�vel por inicializar novamente os componentes necess�rios para que a ronda comece.
    /// </summary>
    void RestartRound()
    {
        _timerController.Play();

        _timerController.SetInitialTime();

        _roundController.DisableNextRoundIntro();

        SetInitialPosition();

        DestroyAllPowerUps();

        InvokeRepeating(nameof(SpawnPowerUp), 5f, 10f);
    }

    void SetInitialPosition()
    {
        _levelPlayers[0].Object.transform.position = _levelPlayers[0].InitialPosition;
        _levelPlayers[0].Object.transform.rotation = _levelPlayers[0].InitialRotation;

        _levelPlayers[1].Object.transform.position = _levelPlayers[1].InitialPosition;
        _levelPlayers[1].Object.transform.rotation = _levelPlayers[1].InitialRotation;

        _ballObject.transform.position = _ballPrefab.transform.position;
        _ballObject.transform.rotation = _ballPrefab.transform.rotation;
    }

    void DestroyAllPowerUps()
    {
        GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject obj in objectsToDestroy)
        {
            Destroy(obj);
        }
    }

    /// <summary>
    /// � executado quando � clicado o bot�o de pr�ximo n�vel, no painel de fim de n�vel.
    /// </summary>
    public void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }
}