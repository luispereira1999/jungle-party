using System.Collections.Generic;
using UnityEngine;

/*
 * Controla o nível 1.
 * O nível consiste em uma partida de futebol com várias rondas.
*/
public class Level1Controller : MonoBehaviour
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

    // para os objetos do nível - bola
    [SerializeField] private GameObject _ballPrefab;
    private GameObject _ballObject;

    // para os objetos do nível - balizas
    [SerializeField] private GameObject _goalPrefab1;
    [SerializeField] private GameObject _goalPrefab2;
    private GameObject _goalObject1;
    private GameObject _goalObject2;

    // para os objetos do nível - power ups
    private readonly List<GameObject> _powerUps = new();
    [SerializeField] private GameObject _powerUp;

    // para definir a ação dos jogadores neste nível
    private KickAction _kickAction;

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




    /* MÉTODOS DO LEVEL4CONTROLLER */

    /*
     * É executado ao clicar no botão de iniciar, no painel de introdução do nível.
    */
    public void InitAfterIntro()
    {

    }
}