using UnityEngine;
using System.Collections;

// A combat system prototype that defines the combat for all objects in game
public class Combat : MonoBehaviour
{
    public Transform hpBar;
    // Three basic variables of combat system
    public float health;
    public float maxHealth;
    public float attack;
    public float defense;

    public float attackCD = 2; // second of cooldown between two attacks
    public float lastHit = -2;
    public string enemyLayer = "";
    public GameObject target = null;
    public bool isImmune;

    public bool[] controlsActivated; //0=dizzy , 1=blind, 2=silent
    // Records all current active buffs
    public ArrayList buffs;

    private void Start()
    {
        health = maxHealth;
        controlsActivated = new bool[3];
        buffs = new ArrayList();
    }

    private void FixedUpdate()
    {
        CheckBuffState();
        CheckHP();
    }

    // Check if each buff is deactivated
    public void CheckBuffState()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            Buff cur = (Buff)buffs[i];
            // Considering the case of different buffs having same control
            if (cur.controlType != -1)
                controlsActivated[cur.controlType] = true;
            if (Time.time >= (cur.startTime + cur.duration))
            {
                OnBuffDeactivate(cur);
                buffs.RemoveAt(i);
            }
        }
    }

    // Receive buffs and active all effects
    public void OnBuffActivate(Buff buff)
    {
        buff.startTime = Time.time;
        buffs.Add(buff);
        // Active all effects on player
        maxHealth += buff.affectedAmount[0];
        attack += buff.affectedAmount[1];
        defense += buff.affectedAmount[2];
        gameObject.GetComponent<Movement>().speed += buff.affectedAmount[3];
        if (buff.controlType != -1)
            controlsActivated[buff.controlType] = true;
    }

    public void OnBuffDeactivate(Buff buff)
    {
        maxHealth -= buff.affectedAmount[0];
        attack -= buff.affectedAmount[1];
        defense -= buff.affectedAmount[2];
        gameObject.GetComponent<Movement>().speed -= buff.affectedAmount[3];
        if (buff.controlType != -1)
            controlsActivated[buff.controlType] = false;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
        try
        {
            // TODO: change logic to avoid using this
            gameObject.GetComponent<Movement>().SetTarget(target);
        }
        catch
        {
            // Debug.Log("Target doesn't contain movement script");
        }
    }

    // Update displayment of HP, magic number for x pos and scale is 4.57
    public void CheckHP()
    {
        float var = 10 * (health / maxHealth);
        hpBar.localScale = new Vector3(var, hpBar.localScale.y, 1);
        hpBar.localPosition = new Vector3((var - 10) / 4.57f, 0, 0);

        if (health <= 0)
        {
            // Destroy Object if dead
            Debug.Log(gameObject.name + " is Dead");
            Destroy(gameObject);
        }
    }

    // Calculate actual damage attack deals to object, return true if attacker killed current obj
    public bool GetHit(float damage, GameObject attacker)
    {
        if (health <= 0 || isImmune)
        {
            return false;
        }
        health -= damage * (1 - defense);
        return health <= 0;
    }

    // Anything that killed this object can call for reward
    public int Loot()
    {
        return 0;
    }
}
