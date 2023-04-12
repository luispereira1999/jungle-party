using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject ButtonPause;

    [SerializeField] private GameObject MenuPause;

    
    public void Pause()
    {
        Time.timeScale = 0f;
        ButtonPause.SetActive(false);
        MenuPause.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        ButtonPause.SetActive(true);
        MenuPause.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        
    }
}
