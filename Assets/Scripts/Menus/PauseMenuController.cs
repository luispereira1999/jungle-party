using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Trata das intera��es do utilizador com o menu de pausa,
 * existente em cada n�vel ao clicar no bot�o de pausa.
*/
public class PauseMenuController : MonoBehaviour
{
    /* ATRIBUTOS */

    // vari�veis para os objetos deste menu
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _menuPause;


    /* M�TODOS */

    public void Pause()
    {
        Time.timeScale = 0f;
        _buttonPause.SetActive(false);
        _menuPause.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        _buttonPause.SetActive(true);
        _menuPause.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        _menuPause.SetActive(false);

        string sceneName = "MenuScene";
        SceneManager.LoadScene(sceneName);
    }
}