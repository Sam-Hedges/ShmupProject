using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int health;
 
    public bool canRegen; // can turn regen on or off
    public int regenAmount; // amount to heal per regen tick
    public float regenTime;  // time between regen ticks in seconds
    public float regenHitDelay; // how long after we are hit can we regen
 
    void Start(){
        health = maxHealth; // start with full health
 
        if(canRegen){
            InvokeRepeating("Regen",regenTime, regenTime);
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public void Hit(int amount){
        if(IsInvoking("Regen")){
            CancelInvoke("Regen"); // we've just been hit Cancel any active regen
        }
 
        health-=amount;  // damage our health var by the amount we we hit by
 
        if(health<=0){
            health = 0; // i don't like healthbars or text showing negatives...just me
            Die(); // and now we die
        }
 
        // If we can regen... resume it after the hit delay
        if(canRegen){
            InvokeRepeating("Regen",regenHitDelay, regenTime);
        }
    }
 
    // Make death a public function so things like insta-kill stuff just bypasses health and kills the target
    public void Die(){
        //Do your deathly deeds here ... add kill/death counters...stats...whatever
        // Destroy or Pool your GameObject (not just this component)
    }
 
    void Regen(){
        health+=regenAmount;
 
        if(health>=maxHealth){
            health = maxHealth; // don't over do it
 
            if(IsInvoking("Regen")){
                CancelInvoke("Regen"); // if we have full health we don't need to regen
            }
        }
    }
}