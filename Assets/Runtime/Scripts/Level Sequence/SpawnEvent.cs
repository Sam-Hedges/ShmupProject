using UnityEngine;

/// <summary>
/// Data container for where and how the enemy spawns and moves
/// </summary>
[System.Serializable]
public class SpawnEvent
{
    // ExposedReference<T> allows us to expose a reference in the Timeline clip
    // that can be set in the inspector when the Timeline asset is used in a scene.
    // This is because the Timeline asset is a ScriptableObject and doesn't 
    // typically don't hold direct references to GameObjects in a scene
    
    public ExposedReference<GameObject> formationPrefab;
    public ExposedReference<GameObject> spline;
}
