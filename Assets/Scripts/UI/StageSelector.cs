using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for switching to different levels(scenes) in the game
/// </summary>
public class StageSelector : MonoBehaviour
{

    /// <summary>
    /// Methods to select different levels or return to main menu
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Mainlevel()
    {
        SceneManager.LoadScene(1);
    }    
    
    public void Testlevel()
    {
        SceneManager.LoadScene(3);
    }


    //Extra stuff if we want to do other modes

    /*
    public void HardMode()
    {
        SceneManager.LoadScene(2);
    }

    public void FreeMovementMode()
    {
        SceneManager.LoadScene(3);
    }
    */
}
