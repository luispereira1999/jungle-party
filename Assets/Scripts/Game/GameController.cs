using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Existe apenas uma inst�ncia desta classe durante a execu��o do jogo.
 * � inicializada no menu (uma vez) e passada de cena para cena (n�vel para n�vel),
 * para preversar dados necess�rios, como a pontua��o ou o estado atual do jogo.
 * Para funcionar corretamente, cada cena deve ter um objeto na hierarquia com este script.
*/
public class GameController : MonoBehaviour
{
    /* ATRIBUTOS PRIVADOS */

    // vari�veis sobre os jogadores
    [SerializeField] private GameObject _player1Prefab;
    [SerializeField] private GameObject _player2Prefab;
    private List<GamePlayerModel> _gamePlayers = new();

    // para identificar o n�vel atual
    [SerializeField] private int _currentLevelID = 1;

    // para guardar uma inst�ncia �nica desta classe
    private static GameController _instance;


    /* PROPRIEDADES P�BLICAS */

    public List<GamePlayerModel> GamePlayers
    {
        get { return _gamePlayers; }
        set { _gamePlayers = value; }
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
        AddPlayer(1, _player1Prefab, 0);
        AddPlayer(2, _player2Prefab, 0);
    }

    void AddPlayer(int id, GameObject prefab, int globalScore)
    {
        _gamePlayers.Add(new GamePlayerModel(id, prefab, globalScore));
    }

    /*
     * � executado quando � clicado o bot�o de pr�ximo n�vel, no painel de fim de n�vel,
     * mas � chamado na fun��o "FinishLevel" no controlador de n�vel.
    */
    public void NextLevel(int scorePlayer1, int scorePlayer2)
    {
        _currentLevelID++;

        UpdateGlobalScore(scorePlayer1, scorePlayer2);

        string sceneName = "Level" + _currentLevelID + "Scene";
        SceneManager.LoadScene(sceneName);

        if (_currentLevelID > 5)
        {
            _currentLevelID = -1;

            sceneName = "MenuScene";
            SceneManager.LoadScene(sceneName);
        }
    }

    void UpdateGlobalScore(int scorePlayer1, int scorePlayer2)
    {
        _gamePlayers[0].GlobalScore += scorePlayer1;
        _gamePlayers[1].GlobalScore += scorePlayer2;
    }

    /*
     * Chamar esta fun��o sempre que um novo jogo iniciar,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        _gamePlayers.Clear();
    }
}