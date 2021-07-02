using UnityEngine;

// Combat logic of minions, they only attack first enemy they see
public class MinionCombat : Combat
{
    public int attackType; // 0=melee, 1=caster
    private Animator animator;
    public void Init(int attackType, string otherside)
    {
        health = maxHealth;
        this.attackType = attackType;
        enemyLayer = otherside;
        animator = gameObject.GetComponent<Animator>();
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        CheckHP();
        if (enemyLayer == null)
        {
            return;
        }
        if (target != null && Time.time >= lastHit + attackCD && target != null)
        {
            switch (attackType)
            {
                case 0:
                    if (MeleeAttack())
                        lastHit = Time.time;
                    break;
                case 1:
                    if (CasterAttack())
                        lastHit = Time.time;
                    break;
            }
            
        }
    }

    // Melee attack at range 1, caster attack at range 3
    // For melee minions, once the attack is triggered, damage is instantly dealt to enemy
    private bool MeleeAttack()
    {
        if (Vector2.Distance(target.transform.position, transform.position) <= 1f)
        {
            animator.SetTrigger("attack");
            target.GetComponent<Combat>().GetHit(attack);
        }
        else
            return false;
        return true;
    }

    // For caster minions, the attack will be a bullet, damaging only after it hits enemy
    private bool CasterAttack()
    {
        if (Vector2.Distance(target.transform.position, transform.position) <= 3)
        {
            Particle particle = ((GameObject)Instantiate(Resources.Load("Prefabs/MinionAttack"), transform.position, transform.rotation)).GetComponent<Particle>();
            particle.Init(target, attack);
            animator.SetTrigger("attack");
        }
        else
            return false;
        return true;
    }
}