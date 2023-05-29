using System.Collections.Generic;
using UnityEngine;


public class Level2Controller : MonoBehaviour
{
    private GameController _gameController;

    private List<LevelPlayerModel> _levelPlayers = new();

    private ThrowLvl2Action _throwLvl2Action;

    [SerializeField] private GameObject _powerUp;

    // referência do controlador do relógio
    private TimerController _timerController;

    private HealthBarController _healthBarController;

    [SerializeField] private ScoreController _scoreController;


    // referência do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // para os componentes da UI - painel de introdução, botão de pause e painel do fim de nível
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _finishedLevelPanel;
    [SerializeField] private GameObject _finishedLevelDescription;

    void Start()
    {
        _gameController = GameController.Instance;

        // TEST: usar isto enquanto � testado apenas o n�vel atual (sem iniciar pelo menu)
        _gameController.GamePlayers = new();
        _gameController.InitiateGame();

        // armazenar dados de cada jogador neste n�vel,
        // sabendo que um jogo tem v�rios n�veis e j� existem dados que passam de n�vel para n�vel, como a pontua��o
        CreatePlayersDataForLevel();

        _healthBarController = HealthBarController.Instance;
        _timerController = TimerController.Instance;
        TimerController.Freeze();

        _roundController.DisplayCurrentRound();

        DisplayObjectInScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerController.HasFinished())
        {
            FinishLevel();
            return;
        }

        foreach (LevelPlayerModel levelPlayer in _levelPlayers)
        {
            bool hasLoose = _healthBarController.HasLoose(levelPlayer.ID);

            if (hasLoose)
            {
                if (levelPlayer.ID == 1)
                {
                    _levelPlayers[1].LevelScore = 100;
                }
                else
                {
                    _levelPlayers[0].LevelScore = 100;
                }
                break;
            }
        }
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

    void AddActionToPlayers()
    {
        _throwLvl2Action = _levelPlayers[0].Object.AddComponent<ThrowLvl2Action>();
        _levelPlayers[0].Object.GetComponent<PlayerController>().SetAction(_throwLvl2Action, this);

        _throwLvl2Action = _levelPlayers[1].Object.AddComponent<ThrowLvl2Action>();
        _levelPlayers[1].Object.GetComponent<PlayerController>().SetAction(_throwLvl2Action, this);
    }

    void SpawnPowerUp()
    {
        System.Random rnd = new();
        int xValue = rnd.Next(42, 58);
        int zValue = rnd.Next(71, 84);

        Instantiate(_powerUp, new Vector3(xValue, 5.5f, zValue), Quaternion.Euler(-90, 0, 0));
    }

    public void InitAfterIntro()
    {
        TimerController.Unfreeze();

        _timerController.PlaySound();

        _buttonPause.SetActive(true);
        Destroy(_introPanel);

        InvokeRepeating(nameof(SpawnPowerUp), 0f, 7f);
    }

    void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }
}
