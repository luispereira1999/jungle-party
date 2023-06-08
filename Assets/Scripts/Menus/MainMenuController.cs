using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Trata das intera��es do utilizador com o menu principal.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    /* ATRIBUTOS */

    // vari�veis para os objetos do menu
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _credits;

    // refer�ncia para o controlador de jogo
    private GameController _game;


    /* M�TODOS */

    void Start()
    {
        _game = GameController.Instance;
    }

    public void Play()
    {
        _game.InitiateGame();

        string sceneName = "Level" + _game.CurrentLevelID + "Scene";
        SceneManager.LoadScene(sceneName);
    }

    public void OpenControls()
    {
        _mainMenu.SetActive(false);
        _controls.SetActive(true);
    }

    public void CloseControls()
    {
        _controls.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public void OpenCredits()
    {
        _mainMenu.SetActive(false);
        _credits.SetActive(true);
    }

    public void CloseCredits()
    {
        _credits.SetActive(false);
        _mainMenu.SetActive(true);
    }
}