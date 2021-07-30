using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Besides normal combats, consider the effects of buffs
public class HeroCombat : Combat
{
    private Skill[] skills; // 0 is auto attack, 1&2 are skills
    private float[] skillLastUsed; // Represent the time skills are last used
    
    private void Start()
    {
        skills = new Skill[3];
        skillLastUsed = new float[3];
        //TODO: Read skills from file

        health = maxHealth;
        controlsActivated = new bool[3];
        buffs = new ArrayList();
        target = null;
    }

    // Target on combat script is only used for attacking and must be enemy
    public void ChangeTarget(GameObject target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        CheckBuffState();
        CheckHP();

        // Control types will also affect behavior
        gameObject.GetComponent<HeroMovement>().SetMoveable(controlsActivated[0]);
        if (!controlsActivated[0])
        {
            if (!controlsActivated[1])
            {
                UseSkill(0);
            }
            if (!controlsActivated[2])
            {
                UseSkill(1);
                UseSkill(2);
            }
        }
    }

    // Use the given skill
    public void UseSkill(int i)
    {
        if (target == null)
        {
            return;
        }
        if (Time.time >= skillLastUsed[i] + skills[i].cd && Vector3.Distance(target.transform.position, transform.position) < skills[i].maxDistance)
        {
            skillLastUsed[i] = Time.time;
            // If is melee, directly do damage
            float actualDamage = skills[i].fixedDamage + attack * skills[i].floatDamage;
            // Directly do damage if is melee
            if (skills[i].isMelee)
            {
                // Also set animator
            } else if (skills[i].isRanged)
            {
                // Activate targeting particle, use prefab associated
            } else
            {
                // Activate a direction based collider
            }

            // Add buff effects
            if (skills[i].hasBuff)
                target.GetComponent<Combat>().OnBuffActivate(skills[i].buff);
        }
    }
}
