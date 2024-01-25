using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame
{
    [CreateAssetMenu]
    public class PhotonBeamAbility : Ability
    {
        public float damagePerSecond = 15f;

        public override void Activate(GameObject parent)
        {
            PlayPhotonBeamVisual();
        }

        private void PlayPhotonBeamVisual()
        {
            Debug.Log("Photon Beam Activated");
        }

        public override void BeginCooldown(GameObject parent)
        {
            Debug.Log("Begin cooldown");   
        }
    }
}
