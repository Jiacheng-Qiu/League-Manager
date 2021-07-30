using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionMovement : Movement
{
    private string otherside;
    private Transform[] nodes;
    private int cur = 0;
    private string side;
    private Animator animator;
    private float range; // Records the range of not moving to attack, 1 for melee and 3 for caster
    private bool stop = false; // This should only be true when the enemy crystal is destroyed

    public void Init(string side, int line)
    {
        this.side = side;
        if (side.Equals("Blue"))
        {
            otherside = "Red";
        }
        else
        {
            otherside = "Blue";
        }
        // Setup all waypoints
        nodes = new Transform[9];
        for (int i = 0; i <= 3; i++)
        {
            nodes[i] = GameObject.Find("Minion Path").transform.Find(side + line + "" + i);
        }
        for (int i = 3; i >= 0; i--)
        {
            nodes[8 - i - 1] = GameObject.Find("Minion Path").transform.Find(otherside + line + "" + i);
        }
        nodes[8] = GameObject.Find("Minion Path").transform.Find(otherside + " Spawn" + line);
        targetPos = nodes[cur++].position;
        animator = gameObject.GetComponent<Animator>();

        range = (gameObject.name.Contains("Melee")) ? 1.2f : 3;
    }

    private void FixedUpdate()
    {
        animator.SetBool("isWalk", false);
        Vector3 directionPos;
        // Won't move if info isn't given yet or destination reached and there are no enemies
        if (side == "" || moveable || (target == null && stop))
        {
            return;
        }
        // Will only move when has a target and is far from it.
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) <= range)
            {
                return;
            }
            // Using target
            directionPos = target.transform.position;
            transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        }
        else
        {
            // Using destination waypoint
            directionPos = targetPos;
            transform.position += (targetPos - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        }

        // Switch gameobject rotation and hp rotation based on target position
        if ((directionPos - transform.position).x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            Vector3 scale = transform.Find("HPBar").localScale;
            transform.Find("HPBar").localScale = new Vector3(Mathf.Abs(scale.x), scale.y, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            Vector3 scale = transform.Find("HPBar").localScale;
            transform.Find("HPBar").localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, 1);
        }

        // After each move, examine if waypoint reached, if so, go to next
        if (Vector3.Distance(targetPos, transform.position) < 0.4f)
        {
            if (cur < nodes.Length)
            {
                targetPos = nodes[cur++].position;
            }
            else if (cur >= nodes.Length)
            {
                stop = true;
            }
        }
    }
}
