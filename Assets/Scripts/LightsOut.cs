using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Collections;

/// <summary>
/// Class for changing lighting in game
/// </summary>
public class LightsOut : MonoBehaviour
{
    [Tooltip("Fog and lighting values for normal and lights out")]
    [SerializeField]    
    private float dark;
    [SerializeField]
    private float normal;
    [SerializeField]
    private float fogDark;
    [SerializeField]
    private float fogNormal;

    private float timer;

    //Lap starts at 25 to give player a period from start to get ready (can be adjusted for difficulty)
    private float lap = 25;

    public float lerp = 0f;
    public float duration = 2f;
    /// <summary>
    /// Method for triggering lights out after an amount of time and setting a new random time. Sets timer to -5 so it doesn't interupt the delta time timer
    /// </summary>
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > lap)
        {
            LightsOff();
            StartCoroutine(Duration());

            timer = -5;
            lap = Random.Range(10, 30);
        }
    }
    /// <summary>
    /// Ienumerator to control blackout duration
    /// </summary>
    /// <returns></returns>
    public IEnumerator Duration()
    {
        yield return new WaitForSeconds(5);

        LightsOn();
    }

    /// <summary>
    /// Methods for changing lighting settings
    /// </summary>
    public void LightsOff()
    {
        RenderSettings.ambientIntensity = dark;
        RenderSettings.fogDensity = fogDark;

    }    
    
    public void LightsOn()
    {
        RenderSettings.ambientIntensity = normal;
        RenderSettings.fogDensity = fogNormal;
    }
}
