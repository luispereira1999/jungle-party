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
    // vari�veis para guardar os prefabs dos jogadores
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public int currentLevelID = -1;

    // para controlar em qual estado e cena o jogo est� no momento
    public GameState gameState = GameState.MAIN_MENU;
    private string sceneName;

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

    }

    /*
     * � executado uma vez por frame.
    */
    void Update()
    {
        // TODO: posteriormente trocar este c�digo, pelo clique do bot�o de iniciar jogo, no menu
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
     * Chamar esta fun��o sempre que um novo jogo se iniciar novamente,
     * para alterar os valores atuais para os valores originais
    */
    void ResetGame()
    {
        currentLevelID = -1;

        // outros dados...
    }
}