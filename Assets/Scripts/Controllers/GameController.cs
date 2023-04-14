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
    public int currentLevelID = -1;

    // para controlar em qual estado e cena o jogo está no momento
    public GameState gameState = GameState.MAIN_MENU;
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

    }

    /*
     * É executado uma vez por frame.
    */
    void Update()
    {
        // TODO: posteriormente trocar este código, pelo clique do botão de iniciar jogo, no menu
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentLevelID = 4;
            sceneName = "Level" + currentLevelID + "Scene";
            ChangeScene(sceneName);
            gameState = GameState.START_GAME;
        }
    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*
     * Chamar esta função sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    void ResetGame()
    {
        currentLevelID = -1;

        // outros dados...
    }
}