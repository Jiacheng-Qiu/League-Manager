using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the behavior of monsters when they get attacked
public class MonsterCombat : Combat
{
    public bool autoHealing; // Feature that monster will heal when they are at base position and have no target
    public float healAmount;
    private float lastHeal;
    
    private void Start()
    {
        health = maxHealth;
        controlsActivated = new bool[3];
        buffs = new ArrayList();
    }

    private void FixedUpdate()
    {
        CheckBuffState();
        CheckHP();
        // Monster won't attack until heroes attack them
        if (target == null && lastAttacker != null)
        {
            target = lastAttacker;
            gameObject.GetComponent<MonsterMovement>().SetTarget(target);
            lastAttacker = null;
        }
        Attack();
        if (autoHealing)
        {
            Heal();
        }
    }

    // Attack the target
    private void Attack()
    {
        if (target == null || target.tag != "Hero")
        {
            return;
        }
        if (Vector3.Distance(target.transform.position, transform.position) <= 1 && Time.time > attackCD + lastHit)
        {
            target.GetComponent<Combat>().GetHit(attack, gameObject);
            lastHit = Time.time;
        }
    }

    // Healing is done on every second
    private void Heal()
    {
        if (Time.time > lastHeal + 1)
        {
            health += healAmount;
            lastHeal = Time.time;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
            autoHealing = false;
        }
    }


}
