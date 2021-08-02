using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Controls the AI logic of NPC heroes to decide current moving strategy
public class Hero : MonoBehaviour
{
    private int role; // 0=jungle, 1=control, 2=damage
    private string heroName;
    private string side;
    // Set current strategy based on position and role
    private int teamStrategy;
    private int strategy;
    public int totalGold; // Shows total gold earned till now (including spent)
    public int gold; // Shows gold currently have
    public float damageDone; // Shows total damage done to heroes
    public int kills; // Record heroes killed
    public int deaths; // Record amount of times died

    private HeroCombat combat;
    private HeroMovement movement;

    public void Init(int role, string heroName, string side)
    {
        // Load all resources and info according to hero name

        this.role = role;
        this.heroName = heroName;
        combat = gameObject.GetComponent<HeroCombat>();
        movement = gameObject.GetComponent<HeroMovement>();
        this.side = side;
    }

    private void FixedUpdate()
    {
        // Not inited
        if (heroName == "")
        {
            return;
        }
        // Add gold each second

        // Check current status, assign new tasks (won't assign new task if previous isn't done)
        // If hero is low on health, stop current task and escape, the limiting amount varies based on strategy
        // TODO: Caution! this fixed update is called before Start() in combat class
        if (combat.health <= 0.2f * combat.maxHealth)
        {
            combat.ChangeTarget(null);
            movement.ChangeTarget(GameObject.Find("Hero Destination").transform.Find(side + "Spawn").gameObject);
        }
        // Otherwise, let hero go for the best target existing, prioritizing: enemy nearby low on HP, tower low on HP(lane champ)/monster low on HP(jungle), crystal(late game for all roles), original target
        else if(combat.target == null && movement.target == null)
        {
            // GO for the monsters existing
            // Same side for farm
            switch (strategy)
            {
                case 0:
                    for (int i = 1; i < 6; i++)
                    {
                        Transform nextTarget = GameObject.Find("Monster Folder").transform.Find(side + i);
                        if (nextTarget != null)
                        {
                            combat.ChangeTarget(nextTarget.gameObject);
                            Debug.Log(nextTarget.transform.position);
                            movement.ChangeTarget(nextTarget.gameObject);
                            break;
                        }
                    }
                    // After checking monsters on same side, if target is still not found, go check goblin
                    if (combat.target == null)
                    {
                        Transform nextTarget = GameObject.Find("Monster Folder").transform.Find("Goblin");
                        if (nextTarget != null)
                        {
                            combat.ChangeTarget(nextTarget.gameObject);
                            movement.ChangeTarget(nextTarget.gameObject);
                        }
                    }
                    break;

                case 2:
                    string otherside = (side == "Red") ? "Blue" : "Red";
                    for (int i = 5; i > 0; i--)
                    {
                        Transform nextTarget = GameObject.Find("Monster Folder").transform.Find(otherside + i);
                        if (nextTarget != null)
                        {
                            combat.ChangeTarget(nextTarget.gameObject);
                            Debug.Log(nextTarget.transform.position);
                            movement.ChangeTarget(nextTarget.gameObject);
                            break;
                        }
                    }
                    // After checking monsters on same side, if target is still not found, go check goblin
                    if (combat.target == null)
                    {
                        Transform nextTarget = GameObject.Find("Monster Folder").transform.Find("Goblin");
                        if (nextTarget != null)
                        {
                            combat.ChangeTarget(nextTarget.gameObject);
                            movement.ChangeTarget(nextTarget.gameObject);
                        }
                    }
                    break;
            }
            
            // If there is still no available target, stay at position TODO
        }
    }

    public void BuyEquipment()
    {

    }

    public void SetTeamStrategy(int strat)
    {
        this.teamStrategy = strat;
    }

    public void SetStrategy(int strat)
    {
        this.strategy = strat;
    }
}
