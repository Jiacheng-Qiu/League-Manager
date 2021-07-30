using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : Movement
{
    private PathFinding pathFinder;
    private ArrayList pathToTarget;

    private void Start()
    {
        pathFinder = new PathFinding();
        pathToTarget = new ArrayList();
    }

    // Assign new moving target, this target can be ally or terrain
    public void ChangeTarget(GameObject target)
    {
        this.target = target;
        pathToTarget = pathFinder.FindPath(transform.position, target.transform.position);
    }

    private void FixedUpdate()
    {
        // Check the position of target, if it's too far from the last node, reassign path
        if (target != null && Vector3.Distance(target.transform.position, transform.position) > 5 && (pathToTarget.Count == 0 || Vector3.Distance(target.transform.position, (Vector3)pathToTarget[pathToTarget.Count - 1]) > 3))
        {
            ChangeTarget(target);
        }
        if (target != null && !moveable)
        {
            Move();
        }
    }

    // Move on the associated path to the target
    private void Move()
    {
        if (target == null || Vector3.Distance(target.transform.position, transform.position) < 1)
        {
            return;
        }
        if (pathToTarget.Count > 0 && Vector3.Distance(transform.position, (Vector3)pathToTarget[0]) < 1)
        {
            pathToTarget.RemoveAt(0);
        }
        if (pathToTarget.Count == 0)
        {
            // Meaning that path is fully used up, go directly to the target as it's close enough
            transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            return;
        }
        transform.position += ((Vector3)pathToTarget[0] - transform.position).normalized * speed * Time.deltaTime;
    }
}
