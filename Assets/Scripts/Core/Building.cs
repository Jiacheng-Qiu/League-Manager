using System.Collections;
using UnityEngine;

// Script for logic of buildings (tower and crystal)
public class Building : Combat
{
    private ArrayList enemyList; // Record all enemies that enters attack range

    private LineRenderer warningLine;
    void Start()
    {
        health = maxHealth;
        enemyList = new ArrayList();
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

    // Switch target if the previous one is dead or outside range by going though the enemy list
    private bool SwitchTarget()
    {
        // First check if tower is destroyed, if so attack won't happen
        CheckHP();
        if (target != null)
        {
            return false;
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            if ((GameObject)enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            } else
            {
                target = (GameObject)enemyList[i];
                enemyList.RemoveAt(i);
                break;
            }
        }
        return true;
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            warningLine.enabled = false;
        } else
        {
            warningLine.enabled = true;
            warningLine.SetPosition(1, target.transform.position + new Vector3(0, 1, 0));
        }
        if (Time.time >= lastHit + attackCD && target != null)
        {
            if (AutoAttack())
            {
                lastHit = Time.time;
            }
        }
        // Determine if target switching is necessary after each frame
        SwitchTarget();

    }

    // Mark first target entering range, and queue all enemies entered range
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Queue if it's minion/hero, in enemy layer
        if ((other.tag == "Minion" || other.tag == "Hero") && other.gameObject.layer == LayerMask.NameToLayer(enemyLayer))
        {
            if (target == null)
            {
                target = other.gameObject;
            } else
            {
                // If having target already, record at end of list
                enemyList.Add(other.gameObject);
            }
        }
    }

    // Mark target as null if it leaves range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.Equals(target))
        {
            target = null;
        } else
        {
            // Remove from enemy list
            for(int i = 0; i < enemyList.Count; i ++)
            {
                if (other.gameObject.Equals((GameObject)enemyList[i]))
                {
                    enemyList.RemoveAt(i);
                    break;
                }
            }
        }
    }

}
