using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Trata das interações do utilizador com o menu de pausa, existente em cada nível.
*/
public class PauseMenuController : MonoBehaviour
{
    // variáveis para os objetos deste menu
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private GameObject menuPause;

    public void Pause()
    {
        Time.timeScale = 0f;
        buttonPause.SetActive(false);
        menuPause.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        buttonPause.SetActive(true);
        menuPause.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        string sceneName = "MenuScene";
        SceneManager.LoadScene(sceneName);
        menuPause.SetActive(false);
    }
}