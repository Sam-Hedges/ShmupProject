using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeGame
{
    public class AbilityHolder : MonoBehaviour
    {
        public Ability ability;
        float cooldownTime;
        float activeTime;
        //public List<Ability> abilitiesList;

        AbilityState state = AbilityState.Ready;

        private void Update()
        {
            switch (state)
            {
                case AbilityState.Ready:
                    ability.Activate(gameObject);
                    state = AbilityState.Active;
                    activeTime = ability.activeTime;
                    break;
                case AbilityState.Active:
                    if (activeTime > 0)
                    {
                        activeTime -= Time.deltaTime;
                    }
                    else
                    {
                        ability.BeginCooldown(gameObject);
                        state = AbilityState.Cooldown;
                        cooldownTime = ability.cooldownTime;
                    }
                    break;

                case AbilityState.Cooldown:
                    if (cooldownTime > 0)
                    {
                        cooldownTime -= Time.deltaTime;
                    }
                    else
                    {
                        state = AbilityState.Ready; 
                    }
                    break;
            }
        }
    }
}