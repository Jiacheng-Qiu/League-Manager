using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Contains necessary information to track progress of each game
public class InfoDisplay : MonoBehaviour
{
    Text[,] battleUI; // team name; total kills; gold; towers
    Text[,,] battleInfo; // kill; gold of each player
    Text time; // In game time track
    private float startTime;

    // Get all info and gameobj necessary for tracking everything
    private void Start()
    {
        startTime = Time.time;

        battleUI = new Text[2, 4];
        Transform parent = transform.Find("BattleUI");
        battleUI[0, 0] = parent.Find("Red Name").GetComponent<Text>();
        battleUI[0, 1] = parent.Find("Red Kill").GetComponent<Text>();
        battleUI[0, 2] = parent.Find("Red Gold").GetComponent<Text>();
        battleUI[0, 3] = parent.Find("Red Tower").GetComponent<Text>();
        battleUI[1, 0] = parent.Find("Blue Name").GetComponent<Text>();
        battleUI[1, 1] = parent.Find("Blue Kill").GetComponent<Text>();
        battleUI[1, 2] = parent.Find("Blue Gold").GetComponent<Text>();
        battleUI[1, 3] = parent.Find("Blue Tower").GetComponent<Text>();

        time = parent.Find("Timer").GetComponent<Text>();

        battleInfo = new Text[2, 3, 2];
        parent = transform.Find("Red Info");
        for (int i = 0; i < 3; i++)
        {
            battleInfo[0, i, 0] = parent.Find("Info" + i).Find("KD").GetComponent<Text>();
            battleInfo[0, i, 1] = parent.Find("Info" + i).Find("Gold").GetComponent<Text>();
        }
        parent = transform.Find("Blue Info");
        for (int i = 0; i < 3; i++)
        {
            battleInfo[0, i, 0] = parent.Find("Info" + i).Find("KD").GetComponent<Text>();
            battleInfo[0, i, 1] = parent.Find("Info" + i).Find("Gold").GetComponent<Text>();
        }
    }

    // Kill will only update when necessary
    public void UpdateKill()
    {
        // Total kill update as sum of all players' kill count
    }

    // Tower will also update when necesssary
    public void UpdateTower()
    {
        
    }

    // Gold and time will update every frame
    private void FixedUpdate()
    {
        // Total gold update as sum of all players' gold

        // Also update time
        int min = (int)((Time.time - startTime) / 60);
        int sec = (int)((Time.time - startTime) % 60);
        time.text = ((min / 10 == 0)? "0" : "") + min + ":" + ((sec / 10 == 0) ? "0" : "") + sec;
    }

}
