using System;
using UnityEngine;

public enum EmissionShapes
{
    Point = 1, // Ensures amount of points is same as index
    Line, 
    Triangle,
    Square,
    Pentagon,
    Hexagon,
    Heptagon,
    Octagon,
    Nonagon,
    Decagon,
    //... can add more shapes
    Circle
}

/// <summary>
/// Handles the spawning logic for every type of spawn shape
/// </summary>
public static class EmissionShapeHelper
{
    /// <summary>
    /// Creates a spawn position and direction for the given shape
    /// </summary>
    /// <returns>
    /// Spawn position is the first two components (x, y)
    /// Spawn direction is the last two components (z, w)
    /// </returns>
    public static Vector4 GetEmissionData(EmissionShapes shape, float arcAngle, float radius, float time)
    {
        Vector2 position;
        Vector2 direction; 

        switch (shape)
        {
            case EmissionShapes.Point:
                position = Vector2.zero;
                direction = Vector2.up;
                break;

            case EmissionShapes.Line:
                position = new Vector2(Mathf.Lerp(-1f, 1f, time), 0) * radius;
                direction = Vector2.up;
                break;
            
            case EmissionShapes.Circle:
                float angle = Mathf.Lerp(0f, arcAngle, time) * Mathf.Deg2Rad;
                position = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                direction = position.normalized;
                break;

            default: // Polygons
                return GetPointOnPolygon((int)shape, time, radius, arcAngle);
        }

        return new Vector4(position.x, position.y, direction.x, direction.y);
    }
    
    public static Vector4 GetPointOnPolygon(int sides, float time, float radius, float arcAngle)
    {
        // Total angle around a polygon
        float totalAngle = 360f;
        
        // Angle per edge of the polygon
        float anglePerSide = totalAngle / sides;
        
        // Calculate the total circumference covered based on time and arcAngle
        float coveredCircumference = arcAngle * time;

        // Find which edge we are on and how far along that edge we are
        int edgeIndex = Mathf.FloorToInt(coveredCircumference / anglePerSide);
        float progressOnEdge = coveredCircumference % anglePerSide / anglePerSide;

        // Calculate start and end points of this edge
        Vector2 startPoint = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * anglePerSide * edgeIndex) * radius,
            Mathf.Sin(Mathf.Deg2Rad * anglePerSide * edgeIndex) * radius
        );
        
        Vector2 endPoint = new Vector2(
            Mathf.Cos(Mathf.Deg2Rad * anglePerSide * (edgeIndex + 1)) * radius,
            Mathf.Sin(Mathf.Deg2Rad * anglePerSide * (edgeIndex + 1)) * radius
        );

        // Calculate position on the edge
        Vector2 position = Vector2.Lerp(startPoint, endPoint, progressOnEdge);

        // Calculate direction as normal of current edge
        Vector2 dir = (endPoint - startPoint).normalized;
        Vector2 normal = new Vector2(dir.y, -dir.x);

        return new Vector4(position.x, position.y, normal.x, normal.y);
    }
    
    public static float Remap (this float value, float from1, float from2, float to1, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
}