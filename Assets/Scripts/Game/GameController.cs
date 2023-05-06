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
    private List<GamePlayerModel> _players = new();

    // para identificar o nível atual
    private int _currentLevelID = 1;

    // para guardar uma instância única desta classe
    private static GameController _instance;


    /* PROPRIEDADES PÚBLICAS */

    public List<GamePlayerModel> Players
    {
        get { return _players; }
        set { _players = value; }
    }

    public int CurrentLevelID
    {
        get { return _currentLevelID; }
        set { _currentLevelID = value; }
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
        AddPlayer(_player1Prefab, 0, 1);
        AddPlayer(_player2Prefab, 0, 2);
    }

    void AddPlayer(GameObject playerPrefab, int score, int id)
    {
        _players.Add(new GamePlayerModel(playerPrefab, score, id));
    }

    public void NextLevel()
    {
        _currentLevelID++;

        if (_currentLevelID > 5)
        {
            _currentLevelID = -1;
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