using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Control script that deal with strategy inputs, and update UI with game info
public class MacroControl : MonoBehaviour
{
    // Record the side that player is on
    public string playerSide;
    private int role; // Another parameter for sendStrategy

    private Transform goldFolder;
    private Transform damageFolder;

    // Record the last update of gold and damage
    private float lastUpdated;

    // These buttons are setup in editor
    public GameObject teamButton;
    public GameObject jgButton;
    public GameObject ctButton;
    public GameObject dmButton;
    private void Start()
    {
        playerSide = "Red";
        // Read the heroes going to be used on each side from files


        // GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Red Name").GetComponent<Text>().text = 
        // GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Blue Name").GetComponent<Text>().text = 
        // First fetch and assign heroes onto specific positions in game
        // Resources.Load("Prefabs/Heroes/");

        // TODO: Put all six heroes on position
        GameObject hero = Instantiate(Resources.Load("Heroes/HeroSample")) as GameObject;
        hero.name = "JG";
        hero.GetComponent<Hero>().Init(0, "Killer", "Red");
        hero.layer = LayerMask.NameToLayer("Red");
        hero.transform.SetParent(GameObject.Find("Hero Folder").transform.Find("Red"));
        hero.transform.position = new Vector3(-14.5f, 6, 0);

        hero = Instantiate(Resources.Load("Heroes/HeroSample")) as GameObject;
        hero.name = "DM";
        hero.GetComponent<Hero>().Init(2, "Sniper", "Red");
        hero.layer = LayerMask.NameToLayer("Red");
        hero.transform.SetParent(GameObject.Find("Hero Folder").transform.Find("Red"));
        hero.transform.position = new Vector3(-13.5f, 5, 0);

        // On startup, set onclick for balance as team strategy, and farm for all heros
        teamButton.GetComponent<Button>().onClick.Invoke();
        jgButton.GetComponent<Button>().onClick.Invoke();
        ctButton.GetComponent<Button>().onClick.Invoke();
        dmButton.GetComponent<Button>().onClick.Invoke();

        goldFolder = GameObject.Find("GameCanvas").transform.Find("Strategy").Find("Gold Chart");
        damageFolder = GameObject.Find("GameCanvas").transform.Find("Strategy").Find("Damage Chart");
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
                transform.Find(playerSide).Find("JG").GetComponent<Hero>().SetStrategy(strat);
                break;
            case 1:
                //transform.Find(playerSide).Find("CT").GetComponent<Hero>().SetStrategy(strat);
                break;
            case 2:
                transform.Find(playerSide).Find("DM").GetComponent<Hero>().SetStrategy(strat);
                break;
        }

    }

    public void SendTeamStrategy(int strat)
    {
        Transform sideFolder = transform.Find(playerSide);
        /*sideFolder.Find("JG").GetComponent<Hero>().SetTeamStrategy(strat);
        sideFolder.Find("CT").GetComponent<Hero>().SetTeamStrategy(strat);
        sideFolder.Find("DM").GetComponent<Hero>().SetTeamStrategy(strat);*/
    }

    private void FixedUpdate()
    {
        // Update KD, stats, and skill info every frame
        UpdateInfo();

        // Update the gold, damage info onto the chart every half second
        if (Time.time < lastUpdated + 0.5f)
        {
            return;
        }
        lastUpdated = Time.time;
        UpdateCharts();
    }

    private void UpdateInfo()
    {
        int redKill = 0;
        int blueKill = 0;
        foreach (Transform child in transform.Find("Red"))
        {
            Transform target = GameObject.Find("GameCanvas").transform.Find("Red Info").Find("Info" + child.name);
            redKill += child.GetComponent<Hero>().kills;
            target.Find("KD").GetComponent<Text>().text = child.GetComponent<Hero>().kills + "/" + child.GetComponent<Hero>().deaths;
            target.Find("Gold").GetComponent<Text>().text = child.GetComponent<Hero>().gold.ToString();
            target.Find("HPBar").GetComponent<RectTransform>().sizeDelta = new Vector2(100 * child.GetComponent<HeroCombat>().health / child.GetComponent<HeroCombat>().maxHealth, 15);
            // TODO Update skill state

            // Update item state
        }
        GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Red Kill").GetComponent<Text>().text = redKill.ToString();
        
        foreach (Transform child in transform.Find("Blue"))
        {
            Transform target = GameObject.Find("GameCanvas").transform.Find("Blue Info").Find("Info" + child.name);
            blueKill += child.GetComponent<Hero>().kills;
            target.Find("KD").GetComponent<Text>().text = child.GetComponent<Hero>().kills + "/" + child.GetComponent<Hero>().deaths;
            target.Find("Gold").GetComponent<Text>().text = child.GetComponent<Hero>().gold.ToString();
            target.Find("HPBar").GetComponent<RectTransform>().sizeDelta = new Vector2(100 * child.GetComponent<HeroCombat>().health / child.GetComponent<HeroCombat>().maxHealth, 15);
            // TODO Update skill state

            // Update item state
        }
        GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Blue Kill").GetComponent<Text>().text = blueKill.ToString();
    }

    // Adjust info on gold and damage charts
    private void UpdateCharts()
    {
        // Find the largest element, set it's length to 100, and all others length adjust based on percentages
        int redGold = 0;
        int blueGold = 0;
        float[] dmg = new float[6];
        float maxDmg = 0;
        foreach (Transform child in transform.Find("Red"))
        {
            redGold += child.GetComponent<Hero>().totalGold;
            if (child.GetComponent<Hero>().damageDone > maxDmg)
            {
                maxDmg = child.GetComponent<Hero>().damageDone;
            }
            switch (child.name)
            {
                case "JG":
                    dmg[0] = child.GetComponent<Hero>().damageDone;
                    break;
                case "CT":
                    dmg[1] = child.GetComponent<Hero>().damageDone;
                    break;
                case "DM":
                    dmg[2] = child.GetComponent<Hero>().damageDone;
                    break;
            }
        }
        foreach (Transform child in transform.Find("Blue"))
        {
            blueGold += child.GetComponent<Hero>().totalGold;
            if (child.GetComponent<Hero>().damageDone > maxDmg)
            {
                maxDmg = child.GetComponent<Hero>().damageDone;
            }
            switch (child.name)
            {
                case "JG":
                    dmg[5] = child.GetComponent<Hero>().damageDone;
                    break;
                case "CT":
                    dmg[4] = child.GetComponent<Hero>().damageDone;
                    break;
                case "DM":
                    dmg[3] = child.GetComponent<Hero>().damageDone;
                    break;
            }
        }

        // Also update gold on top part
        GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Red Gold").GetComponent<Text>().text = redGold.ToString();
        GameObject.Find("GameCanvas").transform.Find("BattleUI").Find("Blue Gold").GetComponent<Text>().text = blueGold.ToString();


        // Adjust the size of the charts, on situation that some dmg is 0, no comparison is needed, simply assign length
        Transform goldChart = GameObject.Find("GameCanvas").transform.Find("Strategy").Find("Gold Chart");
        goldChart.Find("Red Gold").Find("Amount").GetComponent<Text>().text = redGold.ToString();
        goldChart.Find("Blue Gold").Find("Amount").GetComponent<Text>().text = blueGold.ToString();
        if (redGold != 0 && blueGold != 0)
        {
            if (redGold >= blueGold)
            {
                goldChart.Find("Red Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 75);
                goldChart.Find("Blue Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100 * blueGold / redGold, 75);
            }
            else
            {
                goldChart.Find("Blue Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 75);
                goldChart.Find("Red Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100 * redGold / blueGold, 75);
            }
            return;
        }
        else if (redGold != 0)
        {
            goldChart.Find("Blue Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 75);
            goldChart.Find("Red Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 75);
        }
        else if (blueGold != 0)
        {
            goldChart.Find("Red Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 75);
            goldChart.Find("Blue Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 75);
        }
        else
        {
            goldChart.Find("Red Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 75);
            goldChart.Find("Blue Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 75);
        }

        // Modify all gold chart info
        Transform dmgChart = GameObject.Find("GameCanvas").transform.Find("Strategy").Find("Damage Chart");
        for (int i = 0; i < dmg.Length; i++)
        {
            dmgChart.Find(i + " Gold").Find("Amount").GetComponent<Text>().text = dmg[i].ToString();
            if (dmg[i] == 0)
            {
                dmgChart.Find(i + " Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 40);
            }
            else
            {
                dmgChart.Find(i + " Gold").GetComponent<RectTransform>().sizeDelta = new Vector2(100 * dmg[i] / maxDmg, 40);
            }
        }
    }

}
