using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Trata das interações do utilizador com o menu principal.
*/
public class MainMenuController : MonoBehaviour
{
    // variáveis para os objetos do menu
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject controls;
    [SerializeField] private GameObject credits;

    // referência para o controlador de jogo
    private GameController game;


    void Start()
    {
        game = GameController.GetInstance();
    }

    public void Play()
    {
        game.ResetGame();
        game.InitiateGame();

        string sceneName = "Level" + game.currentLevelID + "Scene";
        SceneManager.LoadScene(sceneName);
    }

    public void OpenControls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }

    public void CloseControls()
    {
        controls.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }
}