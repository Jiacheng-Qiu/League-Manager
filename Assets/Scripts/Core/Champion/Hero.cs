using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Controls the AI logic of NPC heroes to decide current moving strategy
public class Hero : MonoBehaviour
{
    // Set current strategy based on position and role
    private int teamStrategy;
    private int strategy;
    private int role; // 0=jungle, 1=control, 2=damage
    public int totalGold; // Shows total gold earned till now (including spent)
    public int gold; // Shows gold currently have
    public float damageDone; // Shows total damage done to heroes
    public int kills; // Record heroes killed
    public int deaths; // Record amount of times died

    public void Init(int role, string heroName)
    {
        // Load all resources and info associated with the hero name
        
    }

    private void FixedUpdate()
    {
        // Check current status, assign new tasks

        // If hero is low on health, run towards base, the limiting amount varies based on strategy

        // Otherwise, let hero go for the best target existing, prioritizing: enemy nearby low on HP, tower low on HP(lane champ)/monster low on HP(jungle), crystal(late game for all roles), original target
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
