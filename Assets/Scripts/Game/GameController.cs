using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Existe apenas uma instância desta classe durante a execução do jogo.
 * É inicializada no menu (uma vez) e passada de cena para cena (nível para nível),
 * para preversar dados necessários, como a pontuação ou o estado atual do jogo.
 * Para funcionar corretamente, cada cena deve ter um objeto na hierarquia com este script.
*/
public class GameController : MonoBehaviour
{
    // variáveis para guardar os prefabs dos jogadores
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public List<PlayerModel> players;
    public int currentLevelID;

    // para controlar em qual estado e cena o jogo está no momento
    public GameState gameState;
    private string sceneName;

    // guarda a instância única desta classe
    private static GameController instance;

    public static GameController GetInstance()
    {
        return instance;
    }


    /*
     * É executado antes da função Start().
    */
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        // guardar em memória apenas uma instância desta classe,
        // e cria-la quando ainda não existe, bem como não destrui-la quando a cena muda.
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    /*
     * É executado antes da primeira frame.
    */
    void Start()
    {
        players = new List<PlayerModel>();
        currentLevelID = -1;
        gameState = GameState.MAIN_MENU;
    }

    /*
     * É executado uma vez por frame.
    */
    void Update()
    {
        // TODO: posteriormente trocar este código, pelo clique do botão de iniciar jogo, do menu
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentLevelID = 4;
            sceneName = "Level" + currentLevelID + "Scene";
            SceneManager.LoadScene(sceneName);
            gameState = GameState.START_GAME;
        }

        // TEST: trocar para o nível seguinte
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("eeeee");

            currentLevelID = 5;
            sceneName = "Level" + currentLevelID + "Scene";
            SceneManager.LoadScene(sceneName);
            gameState = GameState.START_LEVEL;
        }
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

        // usar isto enquanto existe apenas o nível 4
        currentLevelID = 4;

        if (currentLevelID == 5)
        {
            currentLevelID = -1;
            gameState = GameState.FINISH_GAME;
        }
    }

    /*
     * Chamar esta função sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    public void ResetGame()
    {
        players.Clear();
        currentLevelID = -1;
    }
}