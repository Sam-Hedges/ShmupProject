using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Holds a list of SpawnEvents for the timeline sequence and controls the timeline director
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class LevelSequence : MonoBehaviour
{
    public float currentTime;
    // Add methods for playback, scrubbing, etc.
    
    PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.Play();
    }
    
    // Other level sequence logic
}