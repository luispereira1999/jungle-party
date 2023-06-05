using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controla o n�vel 3.
/// O n�vel consiste em empurrar o advers�rio para fora do ringue.
/// O n�vel � constituido por v�rias rondas.
/// </summary>
public class Level3Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _gameController;

    // vari�veis sobre os jogadores
    private List<LevelPlayerModel> _levelPlayers = new();

    // para os objetos do n�vel - power ups
    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;

    // para definir a a��o dos jogadores neste n�vel
    private CarryAction _carryAction;

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

    // para saber se algum jogador saiu da arena e quem foi
    private bool _outOfArena = false;
    private int _playerOutID = -1;


    /* PROPRIEDADES P�BLICAS */

    public bool OutOfArena
    {
        get { return _outOfArena; }
        set { _outOfArena = value; }
    }

    public int PlayerOutID
    {
        get { return _playerOutID; }
        set { _playerOutID = value; }
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
        _roundController.DisplayMaxRounds();

        DisplayObjectInScene();
    }

    void Update()
    {
        // quando est� no intervalo entre rondas, ou seja o tempo est� parado
        if (_timerController.IsOnPause())
        {
            return;
        }

        // se o tempo acabou - congelar objetos, cancelar spawn de power ups, atribuir pontos e iniciar nova ronda
        if (_timerController.HasFinished())
        {
            _timerController.Pause();

            CancelInvoke(nameof(SpawnPowerUp));

            // se estiver na �ltima ronda - mostrar o painel do fim de n�vel
            if (_roundController.IsLastRound())
            {
                // congela para sempre
                FreezePlayers(-1);

                string finishedLevelText = "";
                foreach (LevelPlayerModel levelPlayer in _levelPlayers)
                {
                    finishedLevelText += "Jogador " + levelPlayer.ID + ": " + levelPlayer.LevelScore + "\n";
                }

                _finishedLevelPanel.SetActive(true);
                _finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;

                _buttonPause.SetActive(false);
            }
            // sen�o iniciar outra ronda
            else
            {
                float freezingTime = 5f;
                FreezePlayers(freezingTime);

                _roundController.NextRound();
                _roundController.DisplayNextRoundIntro();
                _roundController.DisplayCurrentRound();

                Invoke(nameof(RestartRound), freezingTime);
            }

            return;
        }

        // se algu�m saiu da arena - congelar objetos, cancelar spawn de power ups, atribuir pontos e iniciar nova ronda
        if (IsOutOfArena())
        {
            _outOfArena = false;

            _timerController.Pause();

            CancelInvoke(nameof(SpawnPowerUp));

            LevelPlayerModel winner = GetWinner();
            UpdateScore(winner.ID);

            // se estiver na �ltima ronda - mostrar o painel do fim de n�vel
            if (_roundController.IsLastRound())
            {
                // congela para sempre
                FreezePlayers(-1);

                string finishedLevelText = "";
                foreach (LevelPlayerModel levelPlayer in _levelPlayers)
                {
                    finishedLevelText += "Jogador " + levelPlayer.ID + ": " + levelPlayer.LevelScore + "\n";
                }

                _finishedLevelPanel.SetActive(true);
                _finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;

                _buttonPause.SetActive(false);
            }
            // sen�o iniciar outra ronda
            else
            {
                float freezingTime = 5f;
                FreezePlayers(freezingTime);

                _roundController.NextRound();
                _roundController.DisplayNextRoundIntro();
                _roundController.DisplayCurrentRound();

                Invoke(nameof(RestartRound), freezingTime);
            }
        }
    }


    /* M�TODOS DO LEVEL3CONTROLLER */

    /// <summary>
    /// � executado ao clicar no bot�o de iniciar, no painel de introdu��o do n�vel.
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
        _carryAction = _levelPlayers[0].Object.AddComponent<CarryAction>();
        _levelPlayers[0].Object.GetComponent<PlayerController>().SetAction(_carryAction, this);

        _carryAction = _levelPlayers[1].Object.AddComponent<CarryAction>();
        _levelPlayers[1].Object.GetComponent<PlayerController>().SetAction(_carryAction, this);
    }

    public bool IsOutOfArena()
    {
        return _outOfArena;
    }

    LevelPlayerModel GetWinner()
    {
        if (_playerOutID == 1)
        {
            return _levelPlayers[1];
        }
        else if (_playerOutID == 2)
        {
            return _levelPlayers[0];
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