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
    public int currentLevelID = -1;
    public GameObject player1Prefab;
    public GameObject player2Prefab;
    public GameState gameState = GameState.MAIN_MENU;
    private string sceneName;

    private static GameController instance;

    public static GameController GetInstance()
    {
        return instance;
    }

    // é chamado antes do Start()
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        // guardar em memória apenas uma instância desta classe,
        // cria-la quando ainda não existe e não destrui-la quando se muda de cena.
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // é chamado antes da primeira frame
    void Start()
    {

    }

    // é chamado uma vez por frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentLevelID = 4;
            sceneName = "Level" + currentLevelID + "Scene";
            ChangeScene(sceneName);
            gameState = GameState.START_GAME;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentLevelID = -1;
            ChangeScene("MainMenuTestScene");
        }
    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /*
     * Chamar esta função sempre que um novo jogo se iniciar,
     * para alterar os valores atuais para os valores originais
    */
    void Reset()
    {
        currentLevelID = -1;

        // outros dados...
    }
}