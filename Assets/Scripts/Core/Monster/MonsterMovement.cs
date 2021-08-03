using UnityEngine;

// Movement of monsters
public class MonsterMovement : Movement
{
    public bool isAlwaysMoving = false; // Only one type of monster is always moving and needs direction instructions
    public int nodeAmount = 8; // Change if there are more or less spots on path
    public Transform basePos;
    private Transform[] nodes; // Only used for needed monsters
    private int currentAim = 0; // Records the current destination on the nodes loop
    private Animator animator;

    private void Start()
    {
        if (isAlwaysMoving)
        {
            // Current default value set is 8 for all
            nodes = new Transform[nodeAmount];
            // Because copies of prefabs will have "clone" marked within
            string monsterName = gameObject.name.Replace("(Clone)", "");
            for (int i = 0; i < nodeAmount; i++)
            {
                nodes[i] = GameObject.Find("Monster Path").transform.Find(monsterName + i);
            }
        }
        animator = gameObject.GetComponent<Animator>();
        moveable = false;
    }

    public new void SetTarget(GameObject target)
    {
        // When a new target is assigned, let healing stop
        gameObject.GetComponent<MonsterCombat>().autoHealing = false;
        this.target = target;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos; // Used for always moving objects to flip around
        animator.SetBool("isWalk", false);
        // In case that monster is affected by dizzy debuff, it stops any movement
        if (moveable)
        {
            return;
        }
        // When monster is moving towards spawn position and have reached it, set target to null
        if (!isAlwaysMoving && Vector3.Distance(basePos.position, transform.position) < 0.5f)
        {
            if (target == basePos.gameObject)
            {
                target = null;
            }
            // When there is no target, start healing
            if (target == null)
            {
                gameObject.GetComponent<MonsterCombat>().autoHealing = true;
            }
        }

        if (isAlwaysMoving && target == null)
        {
            targetPos = nodes[currentAim].position;
            transform.position += (nodes[currentAim].position - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        }
        else if (target != null)
        {
            // TODO
            targetPos = target.transform.position;
            if (Vector3.Distance(target.transform.position, transform.position) <= 0.5f)
            {
                return;
            }
            // Using target
            transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        }else
        {
            targetPos = transform.position;
        }

        // If monster is too far from its spawn, it will go back and reset target
        if (!isAlwaysMoving && Vector3.Distance(basePos.position, transform.position) > 3f)
        {
            target = null;
            gameObject.GetComponent<MonsterCombat>().SetTarget(null);

            // Also move back towards base position
            target = basePos.gameObject;
        }
        // Check if current destination reached, if so go on next
        else if (isAlwaysMoving && Vector3.Distance(nodes[currentAim].position, transform.position) < 0.2f)
        {
            currentAim++;
            currentAim %= nodeAmount; // Only contain 0 to n-1
        }

        // Switch gameobject rotation and hp rotation based on target position
        if ((targetPos - transform.position).x >= 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //transform.Find("HPBar").localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //transform.Find("HPBar").localScale = new Vector3(-1, 1, 1);
        }
    }
}
