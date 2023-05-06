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
    private List<GamePlayerModel> _players = new();

    // para identificar o n�vel atual
    private int _currentLevelID = 1;

    // para guardar uma inst�ncia �nica desta classe
    private static GameController _instance;


    /* PROPRIEDADES P�BLICAS */

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

        // guarda em mem�ria apenas uma inst�ncia desta classe,
        // e cria-la quando ainda n�o existe, tal como n�o destrui-la quando a cena muda.
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
     * Chamar esta fun��o sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        _players.Clear();
    }
}