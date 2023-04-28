using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Trata das interações do utilizador com o menu de pausa,
 * existente em cada nível ao clicar no botão de pausa.
*/
public class PauseMenuController : MonoBehaviour
{
    /* ATRIBUTOS */

    // variáveis para os objetos deste menu
    [SerializeField] private GameObject _buttonPause;
    [SerializeField] private GameObject _menuPause;


    /* MÉTODOS */

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