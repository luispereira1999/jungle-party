using System.Collections.Generic;
using UnityEngine;

/*
 * Existe apenas uma inst�ncia desta classe durante a execu��o do jogo.
 * � inicializada no menu (uma vez) e passada de cena para cena (n�vel para n�vel),
 * para preversar dados necess�rios, como a pontua��o ou o estado atual do jogo.
 * Para funcionar corretamente, cada cena deve ter um objeto na hierarquia com este script.
*/
public class GameController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�veis para guardar os prefabs dos jogadores
    [SerializeField] private GameObject _player1Prefab;
    [SerializeField] private GameObject _player2Prefab;
    private List<PlayerModel> _players = new List<PlayerModel>();
    [SerializeField] private int _currentLevelID;

    // para controlar em qual estado e cena o jogo est� no momento
    [SerializeField] private GameState _gameState = GameState.MAIN_MENU;

    // guarda a inst�ncia �nica desta classe
    private static GameController _instance;


    /* PROPRIEDADES P�BLICAS */

    public List<PlayerModel> Players
    {
        get { return _players; }
        set { _players = value; }
    }

    public int CurrentLevelID
    {
        get { return _currentLevelID; }
        set { _currentLevelID = value; }
    }

    public GameState GameState
    {
        get { return _gameState; }
        set { _gameState = value; }
    }

    public static GameController Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }


    /* M�TODOS */

    /*
     * � executado antes da fun��o Start().
    */
    void Awake()
    {
        if (_instance != null)
        {
            return;
        }

        // guardar em mem�ria apenas uma inst�ncia desta classe,
        // e cria-la quando ainda n�o existe, bem como n�o destrui-la quando a cena muda.
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void InitiateGame()
    {
        AddPlayer(_player1Prefab, 0f);
        AddPlayer(_player2Prefab, 0f);

        _gameState = GameState.START_GAME;
    }

    void AddPlayer(GameObject playerPrefab, float score)
    {
        _players.Add(new PlayerModel(playerPrefab, score));
    }

    public void NextLevel()
    {
        _currentLevelID++;
        _gameState = GameState.START_LEVEL;

        if (_currentLevelID > 5)
        {
            _currentLevelID = -1;
            _gameState = GameState.FINISH_GAME;
        }
    }

    /*
     * Chamar esta fun��o sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        _players.Clear();
    }
}