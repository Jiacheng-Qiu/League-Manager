using UnityEngine;

// A combat system prototype that defines the combat for all objects in game
public class Combat : MonoBehaviour
{
    // Three basic variables of combat system
    public float health;
    public float attack;
    public float defense;

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            // Call death
            Debug.Log(gameObject.name + "is Dead");
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
}
