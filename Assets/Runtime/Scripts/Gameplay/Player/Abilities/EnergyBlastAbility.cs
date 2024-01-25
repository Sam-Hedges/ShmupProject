using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame
{
    [CreateAssetMenu]
    public class EnergyBlastAbility : Ability
    {
        public float blastDamage = 50f;
        public float blastRadius = 5f;

        public override void Activate(GameObject parent)
        {
            Debug.Log("Energy Blast Activated");
            //PlayBlastVisual();
            //Collider[] colliders = Physics.OverlapSphere(parent.transform.position, blastRadius);
            //foreach(Collider col in colliders)
            //{
            //    Enemy enemy = col.GetComponent<Enemy>();
            //    if(enemy != null)
            //    {
            //        enemy.TakeDamage(blastDamage);
            //    }
            //}
        }

        public override void BeginCooldown(GameObject parent)
        {
            Debug.Log("Begin Cooldown");
        }

        private void PlayBlastVisual()
        {
            Debug.Log("Energy Blast Activated");
        }
    }
}
