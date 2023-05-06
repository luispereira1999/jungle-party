using System.Collections.Generic;
using UnityEngine;

/*
 * Controla o n�vel 1.
 * O n�vel consiste em uma partida de futebol com v�rias rondas.
*/
public class Level1Controller : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�vel para a refer�ncia do controlador de jogo
    private GameController _gameController;

    // vari�veis para guardar os jogadores
    private GameObject _player1Object;
    private GameObject _player2Object;
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

    // para detetar que os objetos est�o congelados quando a ronda acaba
    private bool _freezeObjects = false;

    // para o modelo de dados do jogador referente ao n�vel
    private List<LevelPlayerModel> levelPlayers = new();

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




    /* M�TODOS DO LEVEL4CONTROLLER */

    /*
     * � executado ao clicar no bot�o de iniciar, no painel de introdu��o do n�vel.
    */
    public void InitAfterIntro()
    {

    }
}