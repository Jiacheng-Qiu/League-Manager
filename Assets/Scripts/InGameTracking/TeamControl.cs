using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Control script that receives button inputs and send to associated player
public class TeamControl : MonoBehaviour
{
    // Record the side that player is on
    private string side;
    private int role; // Another parameter for sendStrategy
    // These buttons are setup in editor
    public GameObject teamButton;
    public GameObject jgButton;
    public GameObject ctButton;
    public GameObject dmButton;
    private void Start()
    {
        side = "Player";
        // On startup, set onclick for balance as team strategy, and farm for all heros
        teamButton.GetComponent<Button>().onClick.Invoke();
        jgButton.GetComponent<Button>().onClick.Invoke();
        ctButton.GetComponent<Button>().onClick.Invoke();
        dmButton.GetComponent<Button>().onClick.Invoke();
    }

    public void SetRole(int role)
    {
        this.role = role;
    }

    // Role: 0=JG, 1=CT, 3=DM
    public void SendStrategy(int strat)
    {
        switch (role) 
        {
            case 0:
                Debug.Log("Jungle" + strat);
                //transform.Find(side).Find("Jungler").GetComponent<Hero>().SetStrategy(strat);
                break;
            case 1:
                Debug.Log("Controller" + strat);
                //transform.Find(side).Find("Controller").GetComponent<Hero>().SetStrategy(strat);
                break;
            case 2:
                Debug.Log("Damager" + strat);
                //transform.Find(side).Find("Damager").GetComponent<Hero>().SetStrategy(strat);
                break;
        }

    }

    public void SendTeamStrategy(int strat)
    {
        Transform sideFolder = transform.Find(side);
        /*sideFolder.Find("Jungler").GetComponent<Hero>().SetTeamStrategy(strat);
        sideFolder.Find("Controller").GetComponent<Hero>().SetTeamStrategy(strat);
        sideFolder.Find("Damager").GetComponent<Hero>().SetTeamStrategy(strat);*/
    }

    private void FixedUpdate()
    {
        // Update the gold and damage done info onto the chart
        // Find the largest element, set it's length to 100, and all others length adjust based on percentages


    }

}
