using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletSpawner : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private BulletPoolManager bulletPoolManager;
    [SerializeField] private float duration = 5f;
    [SerializeField] private bool looping = false; 
    
    [Header("Bullet")]
    [SerializeField] private BulletPatterns pattern;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private float startSpeed = 5f;
    [SerializeField] private float lifetime = 5f;
    
    [Header("Emission")]
    [SerializeField] private Vector2 emissionDirection;
    [SerializeField] private float spawnRateOverSecond;
    [SerializeField] private Burst[] bursts;
    
    [FormerlySerializedAs("emissionShape")]
    [Header("Shape")]
    [SerializeField] private EmissionShapes formationShape;
    [SerializeField] private float radius = 1f;
    [SerializeField] [Range(0, 360)] public float arcAngle = 360f;
    [SerializeField] private SpawnMode spawnMode;
    [SerializeField] private float burstSpread = 10f; // Degrees between each bullet in a burst
    [SerializeField] private float[] spreadAngles;    // Specific angles to spawn bullets
    
    private float startTime;
    private float nextEmissionTime;
    
    private void Awake() {
        if (pattern != null) { InitBulletPattern(); }
    }
    
    // Initialize the bullet spawner with the bullet pattern data
    private void InitBulletPattern() {
        duration = pattern.duration;
        looping = pattern.looping;
        startSpeed = pattern.startSpeed;
        lifetime = pattern.lifetime;
        emissionDirection = pattern.emissionDirection;
        spawnRateOverSecond = pattern.spawnRateOverSecond;
        bursts = pattern.bursts;
        formationShape = pattern.formationShape;
        radius = pattern.radius;
        arcAngle = pattern.arcAngle;
        spawnMode = pattern.spawnMode;
        burstSpread = pattern.burstSpread;
        spreadAngles = pattern.spreadAngles;
    }
    
    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if ((Time.time - startTime) < duration || looping)
        {
            HandleEmission();
        }
    }
    
    private void HandleEmission()
    {
        if (Time.time >= nextEmissionTime)
        {
            EmitBullet(GetEmissionData());
            nextEmissionTime = Time.time + (1f / spawnRateOverSecond);
        }

        foreach (Burst burst in bursts)
        {
            if (Time.time >= startTime + burst.offsetTime && burst.cyclesCount > 0)
            {
                for (int i = 0; i < burst.bulletCount; i++)
                {
                    Vector4 emissionData = GetEmissionData();
                    Vector2 direction = new Vector2(emissionData.z, emissionData.w);
                    direction = Quaternion.Euler(0, 0, i * burstSpread) * direction;
                    emissionData = new Vector4(emissionData.x, emissionData.y, direction.x, direction.y);
                    EmitBullet(emissionData);
                }

                burst.cyclesCount--;
                burst.offsetTime += burst.intervalBetweenBursts;
            }
        }
    }
    
    private float GetEmissionTime(SpawnMode mode)
    {
        switch (mode)
        {
            case SpawnMode.Loop:
                return (Time.time / spawnRateOverSecond) % 1f;

            case SpawnMode.PingPong:
                return Mathf.PingPong(Time.time / spawnRateOverSecond, 1f);

            case SpawnMode.BurstSpread:
                return 1f / spawnRateOverSecond;

            default:
                return 0f;
        }
    }
    
    private Vector4 GetEmissionData()
    {
        Vector4 emissionData = EmissionShapeHelper.GetEmissionData(formationShape, arcAngle, radius, GetEmissionTime(spawnMode));
        
        Vector2 direction;
        if (emissionDirection != Vector2.zero)
        {
            direction = emissionDirection;
        }
        else {
            direction = new Vector2(emissionData.z, emissionData.w);
        }
        
        // Adjust for specific Spread Angles
        if (spreadAngles.Length > 0)
        {
            float randomAngle = spreadAngles[Random.Range(0, spreadAngles.Length)];
            direction = Quaternion.Euler(0, 0, randomAngle) * direction;
        }
        
        direction.Normalize();
        emissionData = new Vector4(emissionData.x, emissionData.y, direction.x, direction.y);
        
        return emissionData; 
    }

    private void EmitBullet(Vector4 emissionData)
    {
        Vector3 direction = new Vector3(emissionData.z, 0, emissionData.w);
        Vector3 spawnPosition = transform.position + new Vector3(emissionData.x, 0, emissionData.y);;
        Bullet bullet = bulletPoolManager.RequestBullet();
        bullet.transform.position = spawnPosition;
        bullet.Fire(direction, startSpeed, bulletPoolManager);
        StartCoroutine(BulletLifetime(bullet));
    }

    private IEnumerator BulletLifetime(Bullet bullet)
    {
        yield return new WaitForSeconds(lifetime);
        bulletPoolManager.DisableBullet(bullet);
    }
    
    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        DrawShapeGizmo();
    }
    
    private void DrawShapeGizmo()
    {
        switch (formationShape)
        {
            case EmissionShapes.Point:
                Gizmos.DrawSphere(transform.position, 0.1f);
                break;

            case EmissionShapes.Line:
                Gizmos.DrawLine(transform.position - Vector3.right * radius, transform.position + Vector3.right * radius);
                break;
            
            case EmissionShapes.Circle:
                DrawCircleGizmo();
                break;

            default:
                int sides = (int)formationShape;
                
                float totalAngle = 360f;
                
                float anglePerSide = totalAngle / sides;
                    
                int numberOfCompleteEdges = Mathf.FloorToInt(arcAngle / anglePerSide);
                
                float time = 0f;
                float nextSide = 1f / sides;
                
                // Draw complete edges
                for (float i = 0; i < numberOfCompleteEdges; i++)
                {
                    Vector4 point1 = EmissionShapeHelper.GetPointOnPolygon(sides, time, radius, totalAngle);
                    Vector4 point2 = EmissionShapeHelper.GetPointOnPolygon(sides, time + nextSide, radius, totalAngle);
                    Gizmos.DrawLine(transform.position + new Vector3(point1.x, 0, point1.y), transform.position + new Vector3(point2.x, 0, point2.y));
                    time += nextSide;
                }
                
                // Draw partial edge if there is one
                if (arcAngle % anglePerSide != 0)
                {
                    float remainingAngleRatio = (arcAngle % anglePerSide) / anglePerSide;
                    Vector4 point1 = EmissionShapeHelper.GetPointOnPolygon(sides, time, radius, totalAngle);
                    Vector4 point2 = EmissionShapeHelper.GetPointOnPolygon(sides, time + remainingAngleRatio * nextSide, radius, totalAngle);
                    Gizmos.DrawLine(transform.position + new Vector3(point1.x, 0, point1.y), transform.position + new Vector3(point2.x, 0, point2.y));
                }
                break;
        }
    }
    
    private void DrawCircleGizmo()
    {
        float step = arcAngle / 50;  // divide arc into 50 segments for smoothness
        for (float angle = 0; angle < arcAngle; angle += step)
        {
            Vector3 startArcPoint = GetPointOnCircle(angle);
            Vector3 endArcPoint = GetPointOnCircle(angle + step);
            Gizmos.DrawLine(transform.position + startArcPoint, transform.position + endArcPoint);
        }
    }
    
    private Vector3 GetPointOnCircle(float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleInRadians) * radius, 0, Mathf.Sin(angleInRadians) * radius);
    }

    #endregion
}

[System.Serializable]
public class Burst
{
    public float offsetTime;
    public int bulletCount;
    public int cyclesCount;
    public float intervalBetweenBursts;
}

public enum SpawnMode
{
    Loop,
    PingPong,
    BurstSpread
}

