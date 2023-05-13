using System.Collections.Generic;
using UnityEngine;

/*
 * Controla o n�vel 3.
 * O n�vel consiste em empurrar o advers�rio para fora do ringue.
 * O n�vel � constituido por v�rias rondas.
*/
public class Level3Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _gameController;

    // vari�veis sobre os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;

    // para saber se os jogadores colidiram
    private bool _collisionOccurred = false;

    // para definir a a��o dos jogadores neste n�vel
    private CarryAction _carryAction;

    // refer�ncia do controlador do rel�gio
    private TimerController _timerController;

    // refer�ncia do controlador das rondas
    [SerializeField] private RoundController _roundController;

    // para detetar que os objetos est�o congelados quando a ronda acaba
    private bool _freezeObjects = false;

    // para o modelo de dados do jogador referente ao n�vel
    private List<LevelPlayerModel> _levelPlayers = new();

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

            // se estiver na �ltima ronda - mostrar o painel do fim de n�vel
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
        // colis�o com alguma parede da arena - impede que o jogador saia da arena
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
  * Adiciona o script da a��o a cada um dos objetos dos jogadores, para definir essa a��o ao personagem.
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
     * � executado ap�s o intervalo de espera para iniciar outra ronda.
     * Respons�vel por inicializar novamente os componentes necess�rios para que a ronda comece.
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
     * � executado quando � clicado o bot�o de pr�ximo n�vel, no painel de fim de n�vel.
    */
    public void FinishLevel()
    {
        _gameController.NextLevel(_levelPlayers[0].LevelScore, _levelPlayers[1].LevelScore);
    }
}