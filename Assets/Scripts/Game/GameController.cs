using System.Collections.Generic;
using UnityEngine;

/*
 * Existe apenas uma instância desta classe durante a execução do jogo.
 * É inicializada no menu (uma vez) e passada de cena para cena (nível para nível),
 * para preversar dados necessários, como a pontuação ou o estado atual do jogo.
 * Para funcionar corretamente, cada cena deve ter um objeto na hierarquia com este script.
*/
public class GameController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // variáveis para guardar os prefabs dos jogadores
    [SerializeField] private GameObject _player1Prefab;
    [SerializeField] private GameObject _player2Prefab;
    private List<PlayerModel> _players = new List<PlayerModel>();
    [SerializeField] private int _currentLevelID;

    // para controlar em qual estado e cena o jogo está no momento
    [SerializeField] private GameState _gameState = GameState.MAIN_MENU;

    // para guardar uma instância única desta classe
    private static GameController _instance;


    /* PROPRIEDADES PÚBLICAS */

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


    /* MÉTODOS */

    /*
     * É executado antes da função Start().
    */
    void Awake()
    {
        if (_instance != null)
        {
            return;
        }

        // guarda em memória apenas uma instância desta classe,
        // e cria-la quando ainda não existe, tal como não destrui-la quando a cena muda.
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void InitiateGame()
    {
        AddPlayer(_player1Prefab, 0f, 1);
        AddPlayer(_player2Prefab, 0f, 2);

        _gameState = GameState.START_GAME;
    }

    void AddPlayer(GameObject playerPrefab, float score, int id)
    {
        _players.Add(new PlayerModel(playerPrefab, score, id));
    }

    public void NextLevel()
    {
        _currentLevelID++;
        _gameState = GameState.IN_GAME;

        if (_currentLevelID > 5)
        {
            _currentLevelID = -1;
            _gameState = GameState.FINISH_GAME;
        }
    }

    /*
     * Chamar esta função sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        _players.Clear();
    }
}