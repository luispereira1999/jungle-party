using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Existe apenas uma inst�ncia desta classe durante a execu��o do jogo.
 * � inicializada no menu e passada de cena para cena (n�vel para n�vel),
 * para preversar dados necess�rios, como a pontua��o ou o estado atual do jogo.
*/
public class GameController : MonoBehaviour
{
    public int currentLevelID = 0;
    public GameObject player1;
    public GameObject player2;
    private GameState gameState = GameState.MAIN_MENU;
    private string sceneName = "Level4Scene";

    private static GameController instance;

    public static GameController GetInstance()
    {
        return instance;
    }

    // � chamado antes do Start()
    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

        /*
         * Guardar a inst�ncia desta classe, ao mudar de cena.
         * Para funcionar corretamente, cada cena deve ter um objeto na hierarquia com este script.
        */
        DontDestroyOnLoad(this.gameObject);
    }

    // � chamado antes da primeira frame
    void Start()
    {

    }

    // � chamado uma vez por frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            currentLevelID = 4;
            ChangeScene(sceneName);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            currentLevelID = 5;
            ChangeScene("Level5SceneTestScene");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentLevelID = 0;
            ChangeScene("MainMenuTestScene");
        }
    }

    void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}