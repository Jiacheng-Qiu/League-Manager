using UnityEngine;

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

    private void Start()
    {
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        CheckHP();
    }

    // Update displayment of HP, magic number for x pos and scale is 4.57
    public void CheckHP()
    {
        float var = 10 * (health / maxHealth);
        hpBar.localScale = new Vector3(var, 0.9f, 1);
        hpBar.localPosition = new Vector3((var - 10) / 4.57f, 0, 0);

        if (health <= 0)
        {
            // Destroy Object if dead
            Debug.Log(gameObject.name + " is Dead");
            Destroy(gameObject);
        }
    }

    // Basic attack using no skills
    public float Attack()
    {
        return attack;
    }

    // Calculate actual damage attack deals to object, return value shows if attacker killed current obj
    public bool GetHit(float damage)
    {
        if (health <= 0)
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
