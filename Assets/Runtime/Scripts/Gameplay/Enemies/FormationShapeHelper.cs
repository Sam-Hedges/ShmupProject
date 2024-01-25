using System;
using System.Collections.Generic;
using UnityEngine;

public enum FormationShapes
{
    Follow = 1, // Ensures amount of points is same as index
    Circle, 
    Triangle,
    Square,
    Pentagon,
    Hexagon,
    Heptagon,
    Octagon,
    Nonagon,
    Decagon,
    //... can add more shapes
}

public static class FormationShapeHelper
{
    public static IEnumerable<Vector3> GetFormationPoints(FormationShapes shape, FormationManager formationManager)
    {
        switch (shape)
        {
            case FormationShapes.Follow:
                return GeneratePointShape();
            case FormationShapes.Square:
                return GenerateBoxShape(formationManager);
            case FormationShapes.Circle:
                return GenerateCircleShape(formationManager);
            default: // Polygons 
                return GeneratePolygonShape((int)shape, formationManager.radius, formationManager.arcAngle, formationManager.isHollow);
        }
    }

    private static IEnumerable<Vector3> GeneratePointShape()
    {
        yield return Vector3.zero;
    }
    
    private static IEnumerable<Vector3> GenerateBoxShape(FormationManager formationManager) {
        var middleOffset = new Vector3(formationManager.boxWidth * 0.5f, 0, formationManager.boxDepth * 0.5f);

        for (var x = 0; x < formationManager.boxWidth; x++) {
            for (var z = 0; z < formationManager.boxDepth; z++) {
                if (formationManager.isHollow && x != 0 && x != formationManager.boxWidth - 1 && z != 0 && z != formationManager.boxDepth - 1) continue;
                var pos = new Vector3(x + (z % 2 == 0 ? 0 : formationManager.boxNthOffset), 0, z);

                pos -= middleOffset;
                pos += GetNoise(pos, formationManager.noise);
                pos *= formationManager.spread;

                yield return pos;
            }
        }
    }
    
    private static IEnumerable<Vector3> GenerateCircleShape(FormationManager formationManager) {
        var amountPerRing = formationManager.radialAmount / formationManager.rings;
        var ringOffset = 0f;
        for (var i = 0; i < formationManager.rings; i++) {
            for (var j = 0; j < amountPerRing; j++) {
                var angle = j * Mathf.PI * (2 * formationManager.rotations) / amountPerRing + (i % 2 != 0 ? formationManager.radialNthOffset : 0);
                var radius = formationManager.radius + ringOffset + j * formationManager.radiusGrowthMultiplier;
                var x = Mathf.Cos(angle) * radius;
                var z = Mathf.Sin(angle) * radius;

                var pos = new Vector3(x, 0, z);
                pos += GetNoise(pos, formationManager.noise);
                pos *= formationManager.spread;

                yield return pos;
            }

            ringOffset += formationManager.ringOffset;
        }
    }

    private static IEnumerable<Vector3> GeneratePolygonShape(int sides, float radius, float arcAngle, bool isHollow)
    {
        float angleStep = 360f / sides;
        for (int i = 0; i < sides; i++)
        {
            float angle = i * angleStep;
            if (angle <= arcAngle)
            {
                Vector3 outerPoint = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * radius);
                yield return outerPoint;

                if (!isHollow && i < sides - 1)
                {
                    // Generate points towards the center for non-hollow shapes
                    yield return Vector3.zero;
                    yield return outerPoint;
                }
            }
        }
    }
    
    private static Vector3 GetNoise(Vector3 pos, float noise) {
        var noiseValue = Mathf.PerlinNoise(pos.x * noise, pos.z * noise);
        return new Vector3(noiseValue, 0, noiseValue);
    }
}