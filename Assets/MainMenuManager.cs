using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager: MonoBehaviour
{
   [SerializeField] private string levelName;
   [SerializeField] private GameObject MainMenu;
   [SerializeField] private GameObject Controls;
   [SerializeField] private GameObject Credits;

    public void  Play()
    {
        SceneManager.LoadScene(levelName);

    }

    public void OpenControls()
    {
        MainMenu.SetActive(false);
        Controls.SetActive(true);
    }

    public void CloseControls()
    {  
       Controls.SetActive(false);
       MainMenu.SetActive(true);
       
    }
    public void OpenCredits()
    {
        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void CloseCredits()
    {
        Credits.SetActive(false);
        MainMenu.SetActive(true);

    }


}
