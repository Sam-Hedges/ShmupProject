using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BulletPattern", menuName = "Bullet Pattern")]
public class BulletPatterns : ScriptableObject
{
    [Header("General")]
    public float duration = 5f;
    public bool looping = false;
    
    [Header("Bullet")]
    public Bullet bulletPrefab;
    public float startSpeed = 5f;
    public float lifetime = 5f; 
    
    [Header("Emission")]
    public Vector2 emissionDirection;
    public float spawnRateOverSecond;
    public Burst[] bursts;
    
    [FormerlySerializedAs("emissionShape")] [Header("Shape")]
    public EmissionShapes formationShape;
    public float radius = 1f;
    [Range(0, 360)] public float arcAngle = 360f;
    public SpawnMode spawnMode;
    public float burstSpread = 10f; // Degrees between each bullet in a burst
    public float[] spreadAngles;    // Specific angles to spawn bullets
}