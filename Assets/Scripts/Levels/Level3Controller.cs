using System.Collections.Generic;
using UnityEngine;

/*
 * Controla o nível 3.
 * O nível consiste em empurrar o adversário para fora do ringue.
 * O nível é constituido por várias rondas.
*/
public class Level3Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variável para a referência do controlador de jogo
    private GameController _gameController;

    // variáveis sobre os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a ação dos jogadores neste nível
    private CarryAction _carryAction;

    // referência do controlador do relógio
    private TimerController _timerController;

    // referência do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // para detetar que os objetos estão congelados quando a ronda acaba
    private bool _freezeObjects = false;

    // para o modelo de dados do jogador referente ao nível
    private List<LevelPlayerModel> _levelPlayers = new();

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

            // se estiver na última ronda - mostrar o painel do fim de nível
            if (_roundController.IsLastRound())
            {
                string finishedLevelText = "";

                foreach (LevelPlayerModel levelPlayer in _levelPlayers)
                {
                    finishedLevelText += "Jogador " + levelPlayer.ID;
                }

                _finishedLevelPanel.SetActive(true);
                //_finishedLevelDescription.GetComponent<Text>().text = finishedLevelText;

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
    */
    public void InitAfterIntro()
    {
        TimerController.Unfreeze();

        _roundController.NextRound();
        _roundController.DisplayCurrentRound();

        _buttonPause.SetActive(true);
        Destroy(_introPanel);

    }

    void OnTriggerExit(Collider other)
    {
        // colisão com alguma parede da arena - impede que o jogador saia da arena
        if (other.CompareTag("Wall"))
        {
            RestartRound();
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
       
    }

    void SpawnPlayers()
    {
        _levelPlayers[0].Object = Instantiate(_gameController.GamePlayers[0].Prefab);
        _levelPlayers[1].Object = Instantiate(_gameController.GamePlayers[1].Prefab);
    }

    /*
  * Adiciona o script da ação a cada um dos objetos dos jogadores, para definir essa ação ao personagem.
 */
    void AddActionToPlayers()
    {
        _carryAction = _levelPlayers[0].Object.AddComponent<CarryAction>();
        _levelPlayers[0].Object.GetComponent<PlayerController>().SetAction(_carryAction, this);

        _carryAction = _levelPlayers[1].Object.AddComponent<CarryAction>();
        _levelPlayers[1].Object.GetComponent<PlayerController>().SetAction(_carryAction, this);
    }


    void FreezePlayers(float freezingTime)
    {
        _levelPlayers[0].Object.GetComponent<PlayerController>().Freeze(freezingTime);
        _levelPlayers[1].Object.GetComponent<PlayerController>().Freeze(freezingTime);
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

    }

    void SetInitialPosition()
    {
        _levelPlayers[0].Object.transform.position = _levelPlayers[0].InitialPosition;
        _levelPlayers[0].Object.transform.rotation = _levelPlayers[0].InitialRotation;

        _levelPlayers[1].Object.transform.position = _levelPlayers[1].InitialPosition;
        _levelPlayers[1].Object.transform.rotation = _levelPlayers[1].InitialRotation;
    }

    /*
     * É executado quando é clicado o botão de próximo nível, no painel de fim de nível.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }
}