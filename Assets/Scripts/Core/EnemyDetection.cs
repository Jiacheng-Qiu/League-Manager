using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enemy detection attached onto triggers, help AI detect potentional enemies
public class EnemyDetection : MonoBehaviour
{
    private ArrayList enemyList; // Record all enemies that enters attack range
    private Combat combat;
    private int objectType = -1; // Assign type, building = 0, minion = 1

    private void Start()
    {
        enemyList = new ArrayList();
        if (transform.parent.tag.Equals("Structure"))
        {
            objectType = 0;
            combat = transform.parent.GetComponent<Building>();
        } else if (transform.parent.tag.Equals("Minion"))
        {
            objectType = 1;
            combat = transform.parent.GetComponent<MinionCombat>();
        }
    }

    // Mark first target entering range, and queue all enemies entered range
    private void OnTriggerEnter2D(Collider2D other)
    {
        // When the scripts are not yet assigned, stop detection
        if (combat == null)
        {
            return;
        }
        // Queue if it's minion/hero, in enemy layer
        if (other.gameObject.layer == LayerMask.NameToLayer(combat.enemyLayer))
        {
            // For all cases these tags work
            if (other.tag == "Minion" || other.tag == "Structure")
            {
                if (combat.GetTarget() == null)
                {
                    combat.SetTarget(other.gameObject);
                }
                else
                {
                    // If having target already, record at end of list
                    enemyList.Add(other.gameObject);
                }
            }
            // For buildings they will also attack heros
            else if (objectType == 0 &&  other.tag == "Hero")
            {
                if (combat.GetTarget() == null)
                {
                    combat.SetTarget(other.gameObject);
                }
                else
                {
                    // If having target already, record at end of list
                    enemyList.Add(other.gameObject);
                }
            }
        }
    }

    // Mark target as null if it leaves range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.Equals(combat.GetTarget()))
        {
            combat.SetTarget(null);
        }
        else
        {
            // Remove from enemy list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (other.gameObject.Equals((GameObject)enemyList[i]))
                {
                    enemyList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Determine if target switching is necessary after each frame
        SwitchTarget();
    }

    // Switch target if the previous one is dead or outside range by going though the enemy list
    private bool SwitchTarget()
    {
        if (combat.GetTarget() != null)
        {
            return false;
        }
        for (int i = 0; i < enemyList.Count; i++)
        {
            // if element in list becomes null, delete it
            if ((GameObject)enemyList[i] == null)
            {
                enemyList.RemoveAt(i);
            }
            else
            {
                combat.SetTarget((GameObject)enemyList[i]);
                enemyList.RemoveAt(i);
                break;
            }
        }
        return true;
    }
}
