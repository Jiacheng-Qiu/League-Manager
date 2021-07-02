using System.Collections;
using UnityEngine;

// Script for logic of buildings (tower and crystal)
public class Building : Combat
{
    private LineRenderer warningLine;
    void Start()
    {
        health = maxHealth;
        this.defense = 0; // Tower has no defense, which means they take full damage

        // Setup the warning line, and disable it
        warningLine = gameObject.GetComponent<LineRenderer>();
        warningLine.startColor = Color.red;
        warningLine.endColor = Color.red;
        warningLine.startWidth = .01f;
        warningLine.endWidth = .01f;
        warningLine.SetPosition(0, transform.position + new Vector3(0, 1, 0));
        warningLine.useWorldSpace = true;
        warningLine.sortingOrder = 9;
        warningLine.enabled = false;
    }

    // All of them auto attack the first enemy that enters range
    private bool AutoAttack()
    {
        // This if is for the case when the enemy target dies before tower hit him.
        if (target != null)
        {
            Particle particle = ((GameObject)Instantiate(Resources.Load("Prefabs/FireBall"), transform.position + new Vector3(0, 1, 0), transform.rotation)).GetComponent<Particle>();
            particle.Init(target, attack);
            return true;
        } else
        {
            return false;
        }
    }

    

    private void FixedUpdate()
    {
        // First check if tower is destroyed, if so attack won't happen
        CheckHP();
        if (target == null)
        {
            warningLine.enabled = false;
        } else
        {
            warningLine.enabled = true;
            warningLine.SetPosition(1, target.transform.position);
        }
        if (Time.time >= lastHit + attackCD && target != null)
        {
            if (AutoAttack())
            {
                lastHit = Time.time;
            }
        }
    }

}
