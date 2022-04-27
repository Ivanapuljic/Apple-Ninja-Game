using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ToGame()
    {
       
        SceneManager.LoadScene("Game");
    }
    public void ToMenu()
    {
        
        SceneManager.LoadScene("Menu");

    }
    public void ToAbout()
    {
       
        SceneManager.LoadScene("About");

    }
    public void Exit()
    {
        
            Application.Quit();
        
    }
}
