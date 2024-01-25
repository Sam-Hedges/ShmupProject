using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FormationManager))]
public class FormationManagerEditor : Editor
{
    SerializedProperty unitPoolProp;
    SerializedProperty unitPrefabProp;
    SerializedProperty unitSpeedProp;
    SerializedProperty formationShapeProp;
    SerializedProperty isHollowProp;
    SerializedProperty spreadProp;
    SerializedProperty radiusProp;
    SerializedProperty arcAngleProp;
    
    // BoxFormation properties
    SerializedProperty boxWidthProp;
    SerializedProperty boxDepthProp;
    SerializedProperty boxNthOffsetProp;
    
    // RadialFormation properties
    SerializedProperty radialAmountProp;
    SerializedProperty radiusGrowthMultiplierProp;
    SerializedProperty rotationsProp;
    SerializedProperty ringsProp;
    SerializedProperty ringOffsetProp;
    SerializedProperty radialNthOffsetProp;
    
    // ExampleArmy properties
    SerializedProperty unitGizmoSizeProp;
    SerializedProperty gizmoColorProp;
    // ... other properties

    private void OnEnable()
    {
        // Setup the SerializedProperties
        unitPoolProp = serializedObject.FindProperty("pool");
        unitPrefabProp = serializedObject.FindProperty("unitPrefab");
        unitSpeedProp = serializedObject.FindProperty("unitSpeed");
        formationShapeProp = serializedObject.FindProperty("formationShape");
        isHollowProp = serializedObject.FindProperty("isHollow");
        spreadProp = serializedObject.FindProperty("spread");
        radiusProp = serializedObject.FindProperty("radius");
        arcAngleProp = serializedObject.FindProperty("arcAngle");
        
        // BoxFormation properties
        boxWidthProp = serializedObject.FindProperty("boxWidth");
        boxDepthProp = serializedObject.FindProperty("boxDepth");
        boxNthOffsetProp = serializedObject.FindProperty("boxNthOffset");
        
        // RadialFormation properties
        radialAmountProp = serializedObject.FindProperty("radialAmount");
        radiusGrowthMultiplierProp = serializedObject.FindProperty("radiusGrowthMultiplier");
        rotationsProp = serializedObject.FindProperty("rotations");
        ringsProp = serializedObject.FindProperty("rings");
        ringOffsetProp = serializedObject.FindProperty("ringOffset");
        radialNthOffsetProp = serializedObject.FindProperty("radialNthOffset");
        
        // ... other properties
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(unitPoolProp);
        EditorGUILayout.PropertyField(unitPrefabProp);
        EditorGUILayout.PropertyField(unitSpeedProp);

        EditorGUILayout.PropertyField(formationShapeProp);
        EditorGUILayout.PropertyField(isHollowProp);

        FormationShapes shape = (FormationShapes)formationShapeProp.enumValueIndex + 1;

        switch (shape)
        {
            case FormationShapes.Follow:
                // Display properties relevant to Line shape
                break;
            case FormationShapes.Square:
                EditorGUILayout.PropertyField(boxWidthProp);
                EditorGUILayout.PropertyField(boxDepthProp);
                EditorGUILayout.PropertyField(boxNthOffsetProp);
                break;
            case FormationShapes.Circle:
                EditorGUILayout.PropertyField(radialAmountProp);
                EditorGUILayout.PropertyField(radiusGrowthMultiplierProp);
                EditorGUILayout.PropertyField(rotationsProp);
                EditorGUILayout.PropertyField(ringsProp);
                EditorGUILayout.PropertyField(ringOffsetProp);
                EditorGUILayout.PropertyField(radialNthOffsetProp);
                break;
            default:
                EditorGUILayout.PropertyField(radiusProp);
                EditorGUILayout.PropertyField(arcAngleProp);
                break;
        }

        // Always display these properties
        EditorGUILayout.PropertyField(spreadProp);

        serializedObject.ApplyModifiedProperties();
    }
}
