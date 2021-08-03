using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public float cd;
    public bool isMelee; // Melee attack deals damage instantly
    public bool isRanged; // Controls if ability is ranged
    public float range; // If is ranged attack, controls size of collider
    public float maxDistance; // Control the max distance the skill can reach (for melee and ranged)
    public float moveSpeed; // Controls the speed of moving skill collider (for ranged)

    public bool onAlly; // If true the skill will only affect targets on same side
    public bool hasBuff;
    public Buff buff; // Skills can create buffs on targets
    public float fixedDamage; // The fixed amount of damage skill does on target (negative=heal)
    public float floatDamage; // Amount of damage that changes based on hero attack (by percentage)
}
