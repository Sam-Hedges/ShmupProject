using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FormationManager : MonoBehaviour {
    
    // FormationBase properties
    [SerializeField] private EnemyPoolManager pool;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private float unitSpeed = 2;
    [Range(0, 1)] public float noise = 0;
    public float spread = 1;
    
    // Formation properties
    [SerializeField] private FormationShapes formationShape = FormationShapes.Square;
    public bool isHollow = false;
    public float radius = 1f;
    [Range(0, 360)] public float arcAngle = 360f;

    // BoxFormation properties
    public int boxWidth = 5;
    public int boxDepth = 5;
    public float boxNthOffset = 0;

    // RadialFormation properties
    public int radialAmount = 10;
    public float radiusGrowthMultiplier = 0;
    public float rotations = 1;
    public int rings = 1;
    public float ringOffset = 1;
    public float radialNthOffset = 0;

    // ExampleArmy properties
    private readonly List<GameObject> spawnedUnits = new List<GameObject>();
    private List<Vector3> points = new List<Vector3>();

    // FormationRenderer properties
    [SerializeField] private Vector3 unitGizmoSize = Vector3.one;
    [SerializeField] private Color gizmoColor;

    private void Update() {
        SetFormation();
    }

    private void OnEnable()
    {
        SetFormation();
    }

    private void OnDisable()
    {
        // Kill all units
        Kill(spawnedUnits.Count);
    }

    private void SetFormation() {
        points = EvaluatePoints().ToList();

        if (points.Count > spawnedUnits.Count) {
            var remainingPoints = points.Skip(spawnedUnits.Count);
            Spawn(remainingPoints);
        } else if (points.Count < spawnedUnits.Count) {
            Kill(spawnedUnits.Count - points.Count);
        }

        for (var i = 0; i < spawnedUnits.Count; i++) {
            Vector3 worldPoint = transform.TransformPoint(points[i]);
            spawnedUnits[i].transform.position = Vector3.MoveTowards(spawnedUnits[i].transform.position, worldPoint, unitSpeed * Time.deltaTime);
        }
    }

    private void Spawn(IEnumerable<Vector3> points) {
        foreach (var pos in points) {
            Vector3 worldPos = transform.TransformPoint(pos);
            Enemy enemy = pool.RequestEnemy();
            GameObject unit = enemy.gameObject;
            enemy.SetManager(pool);
            unit.transform.position = worldPos;
            // Instantiate(unitPrefab, worldPos, Quaternion.identity, transform);
            spawnedUnits.Add(unit);
        }
    }

    private void Kill(int num) {
        for (var i = 0; i < num; i++) {
            var unit = spawnedUnits.Last();
            pool.DisableEnemy(unit.GetComponent<Enemy>());
            spawnedUnits.Remove(unit);
        }
    }

    private IEnumerable<Vector3> EvaluatePoints() {
        return FormationShapeHelper.GetFormationPoints(formationShape, this);
    }

    private void OnDrawGizmos() {
        if (Application.isPlaying) return;
        Gizmos.color = gizmoColor;

        foreach (var pos in EvaluatePoints()) {
            Vector3 worldPos = transform.TransformPoint(pos + new Vector3(0, unitGizmoSize.y * 0.5f, 0));
            Gizmos.DrawCube(worldPos, unitGizmoSize);
        }
    }
}
