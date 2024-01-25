using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame
{
    [System.Serializable]
    public class SpawnGroupList
    {
        public List<SpawnGroup> spawnGroups;
        
    }
    
    [System.Serializable]
    public class SpawnGroup
    {
        // Contains enemies to spawn and their cinemachine path
        public GameObject enemyGroupPrefab;
        
    }
}
