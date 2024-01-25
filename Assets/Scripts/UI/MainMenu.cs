using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class for main menu Canvas
/// </summary>
public class MainMenu : MonoBehaviour
{
    public OptionsMenu options;
    private void Start()
    {
        SaveSystem.LoadOptions(options);
        options.ChangeAA(options.AAmode);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    /// <summary>
    /// Method to exit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
