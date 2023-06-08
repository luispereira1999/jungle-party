using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controla o nível 2.
/// O nível consiste em apanhar maçãs do cháo e atirá-las ao inimigo.
/// O nível é constituido por várias rondas.
/// </summary>
public class Level2Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _gameController;

    // variáveis sobre os jogadores
    private List<LevelPlayerModel> _levelPlayers = new();

    // para os objetos do nível - maçã
    [SerializeField] private GameObject _apple;

    // para definir a ação dos jogadores neste nível
    private ThrowLvl2Action _throwLvl2Action;

    // referência do controlador do relógio
    private TimerController _timerController;

    // referência do controlador da pontuação
    [SerializeField] private ScoreController _scoreController;

    // referência do controladores da barra de vida
    private HealthBarController _healthBarController;

    // para para o som de acertar com a maçã
    private AudioSource _audioSource;

    // para os componentes da UI - painel de introdução, botão de pause e painel do fim de nível
    [SerializeField] private GameObject _introPanel;
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _finishedLevelPanel;
    [SerializeField] private GameObject _finishedLevelDescription;

    // para o tempo extra que continua depois do tempo normal
    private bool _isExtraTime;


    /* MÉTODOS */

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

        DisplayObjectInScene();

        _isExtraTime = false;

        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_timerController.HasFinished())
        {
            if (_isExtraTime)
            {
                FinishLevel();
                return;
            }
            else
            {
                _timerController.SetExtraTime();
                _isExtraTime = true;
            }
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

    /// <summary>
    /// É executado ao clicar no botão de iniciar, no painel de introdução do nível.
    /// </summary>
    public void InitAfterIntro()
    {
        TimerController.Unfreeze();

        _buttonPause.SetActive(true);
        Destroy(_introPanel);

        InvokeRepeating(nameof(SpawnApple), 0f, 7f);
    }

    void SpawnApple()
    {
        System.Random rnd = new();
        int xValue = rnd.Next(42, 58);
        int zValue = rnd.Next(71, 84);

        Instantiate(_apple, new Vector3(xValue, 5.5f, zValue), Quaternion.Euler(-90, 0, 0));
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

    /// <summary>
    /// É executado quando é clicado o botão de próximo nível, no painel de fim de nível.
    /// </summary>
    public void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }

    public void PlaySplashSound()
    {
        _audioSource.Play();
    }
}