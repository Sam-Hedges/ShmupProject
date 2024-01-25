using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Splines;

[CustomEditor(typeof(SpawnEventClip))]
public class SpawnEventClipEditor : Editor
{
    private List<Editor> splineContainerEditors = new List<Editor>();
    private MethodInfo onInspectorGUIMethod;

    public override void OnInspectorGUI()
    {
        // Draw the default inspector of SpawnEventClip
        DrawDefaultInspector();

    }
}
