using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager: MonoBehaviour
{
   
   [SerializeField] private GameObject mainMenu;
   [SerializeField] private GameObject controls;
   [SerializeField] private GameObject credits;

    public void  Play()
    {
      
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

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
