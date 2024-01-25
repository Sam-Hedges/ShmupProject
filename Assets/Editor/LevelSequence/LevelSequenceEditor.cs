using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Used to draw gizmos for the LevelSequence in the scene view
/// </summary>
[CustomEditor(typeof(LevelSequence))]
public class LevelSequenceEditor : Editor
{
    private PlayableDirector director;
    void OnSceneGUI()
    {
        LevelSequence levelSequence = (LevelSequence)target;
        
    }
}