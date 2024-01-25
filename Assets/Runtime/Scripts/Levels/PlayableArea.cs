using UnityEngine;

[ExecuteInEditMode]
public class PlayableArea : MonoBehaviour
{
    public Vector2 size = new Vector2(10f, 10f); // Size of the playable area
    public float wallThickness = 0.1f; // Thickness of the walls
    public Color color = Color.green; // Color of the playable area

    private GameObject[] walls = new GameObject[4];
    private string[] wallNames = { "Top Wall", "Bottom Wall", "Left Wall", "Right Wall" };

    void OnValidate()
    {
        UpdateWalls();
    }

    private void UpdateWalls()
    {
        // Adjust the size and position of the walls based on the wall thickness
        walls[0] = CreateOrUpdateWall(walls[0], wallNames[0], new Vector3(0, 0, size.y / 2), new Vector3(size.x + 2 * wallThickness, 1, wallThickness)); // Top wall
        walls[1] = CreateOrUpdateWall(walls[1], wallNames[1], new Vector3(0, 0, -size.y / 2), new Vector3(size.x + 2 * wallThickness, 1, wallThickness)); // Bottom wall
        walls[2] = CreateOrUpdateWall(walls[2], wallNames[2], new Vector3(-size.x / 2 - wallThickness / 2, 0, 0), new Vector3(wallThickness, 1, size.y)); // Left wall
        walls[3] = CreateOrUpdateWall(walls[3], wallNames[3], new Vector3(size.x / 2 + wallThickness / 2, 0, 0), new Vector3(wallThickness, 1, size.y)); // Right wall
    }

    private GameObject CreateOrUpdateWall(GameObject wall, string name, Vector3 position, Vector3 scale)
    {
        if (wall == null)
        {
            wall = GameObject.Find(name);
            if (wall == null)
            {
                wall = new GameObject(name);
                wall.transform.parent = transform;
                wall.AddComponent<BoxCollider>();
            }
        }

        wall.transform.localPosition = position;
        wall.transform.localScale = scale;

        return wall;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Vector3 drawPoint1 = transform.position + new Vector3(-size.x / 2 - wallThickness / 2, 0, -size.y / 2 - wallThickness / 2);
        Vector3 drawPoint2 = transform.position + new Vector3(size.x / 2 + wallThickness / 2, 0, -size.y / 2 - wallThickness / 2);
        Vector3 drawPoint3 = transform.position + new Vector3(size.x / 2 + wallThickness / 2, 0, size.y / 2 + wallThickness / 2);
        Vector3 drawPoint4 = transform.position + new Vector3(-size.x / 2 - wallThickness / 2, 0, size.y / 2 + wallThickness / 2);

        // Draw a box representing the playable area
        Gizmos.DrawLine(drawPoint1, drawPoint2);
        Gizmos.DrawLine(drawPoint2, drawPoint3);
        Gizmos.DrawLine(drawPoint3, drawPoint4);
        Gizmos.DrawLine(drawPoint4, drawPoint1);
    }
}
