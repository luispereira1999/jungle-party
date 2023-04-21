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
    // vari�veis para guardar os prefabs dos jogadores
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public List<PlayerModel> players;
    public int currentLevelID;

    // para controlar em qual estado e cena o jogo est� no momento
    public GameState gameState;

    // guarda a inst�ncia �nica desta classe
    private static GameController instance;

    public static GameController GetInstance()
    {
        return instance;
    }


    /*
     * � executado antes da fun��o Start().
    */
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        // guardar em mem�ria apenas uma inst�ncia desta classe,
        // e cria-la quando ainda n�o existe, bem como n�o destrui-la quando a cena muda.
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /*
     * � executado antes da primeira frame.
    */
    void Start()
    {
        players = new List<PlayerModel>();
        currentLevelID = -1;
        gameState = GameState.MAIN_MENU;
    }

    /*
     * � executado uma vez por frame.
    */
    void Update()
    {

    }

    public void InitiateGame()
    {
        AddPlayer(player1Prefab, 0f);
        AddPlayer(player2Prefab, 0f);

        NextLevel();
        gameState = GameState.START_GAME;
    }

    void AddPlayer(GameObject playerPrefab, float score)
    {
        players.Add(new PlayerModel(playerPrefab, score));
    }

    public void NextLevel()
    {
        currentLevelID++;
        gameState = GameState.START_LEVEL;

        // TEST: usar isto enquanto existe apenas o n�vel 4
        currentLevelID = 4;

        if (currentLevelID > 5)
        {
            currentLevelID = -1;
            gameState = GameState.FINISH_GAME;
        }
    }

    /*
     * Chamar esta fun��o sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        players.Clear();
        currentLevelID = -1;
    }
}