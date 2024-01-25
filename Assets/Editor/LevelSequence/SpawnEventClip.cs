using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Splines;

/// <summary>
/// 
/// </summary>
[System.Serializable]
public class SpawnEventClip : PlayableAsset, ITimelineClipAsset
{
    public List<SpawnEvent> spawnEvents = new List<SpawnEvent>();
    
    // Describes the timeline features supported by a clip
    public ClipCaps clipCaps => ClipCaps.None;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        foreach (SpawnEvent sEvent in spawnEvents)
        {
            ScriptPlayable<SpawnEventPlayableBehavior> playable = ScriptPlayable<SpawnEventPlayableBehavior>.Create(graph);
            SpawnEventPlayableBehavior behavior = playable.GetBehaviour();
            behavior.spawnEvent = sEvent;
            return playable;
        }
        return Playable.Null;
    }
}

/// <summary>
/// 
/// </summary>
public class SpawnEventPlayableBehavior : PlayableBehaviour
{
    public SpawnEvent spawnEvent;
    private bool hasBeenInitialized = false;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (!hasBeenInitialized)
        {
            Initialize(playable);
            hasBeenInitialized = true;
        }

        UpdateSplineAnimation(playable);
    }

    private void Initialize(Playable playable)
    {
        // Assuming SplineAnimate is a component in your project
        SplineAnimate splineAnimate = spawnEvent.formationPrefab.Resolve(playable.GetGraph().GetResolver()).GetComponent<SplineAnimate>();
        if (splineAnimate != null)
        {
            splineAnimate.Duration = (float)playable.GetDuration();
        }

        // Enable the spline GameObject
        GameObject splineGameObject = spawnEvent.spline.Resolve(playable.GetGraph().GetResolver());
        if (splineGameObject != null)
        {
            splineGameObject.SetActive(true);
        }
    }

    private void UpdateSplineAnimation(Playable playable)
    {
        SplineAnimate splineAnimate = spawnEvent.formationPrefab.Resolve(playable.GetGraph().GetResolver()).GetComponent<SplineAnimate>();
        if (splineAnimate != null)
        {
            double clipDuration = playable.GetDuration();
            double currentTime = playable.GetTime();

            // Normalize currentTime to the range of 0 to 1
            float normalizedTime = (float)(currentTime / clipDuration);

            // Assuming the 'time' property of SplineAnimate is meant to be set as a normalized value
            splineAnimate.ElapsedTime = normalizedTime * splineAnimate.Duration;
        }
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        // Enable the spline GameObject
        GameObject splineGameObject = spawnEvent.spline.Resolve(playable.GetGraph().GetResolver());
        if (splineGameObject != null)
        {
            splineGameObject.SetActive(true);
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        // Disable the spline GameObject
        GameObject splineGameObject = spawnEvent.spline.Resolve(playable.GetGraph().GetResolver());
        if (splineGameObject != null)
        {
            splineGameObject.SetActive(false);
        }
    }
}