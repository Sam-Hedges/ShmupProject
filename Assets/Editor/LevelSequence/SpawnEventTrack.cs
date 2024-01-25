using UnityEngine.Timeline;

/// <summary>
/// The track within the timeline that holds the SpawnEventClips
/// </summary>
[TrackColor(0.85f, 0.56f, 0.44f)]
[TrackClipType(typeof(SpawnEventClip))]
//[TrackBindingType(typeof(LevelSequence))] 
public class SpawnEventTrack : TrackAsset
{
    // This track doesn't need any custom logic for this example, but you can expand upon it if needed
}