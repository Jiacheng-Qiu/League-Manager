using UnityEngine;

// General movement class define behavior for all AI
public class Movement : MonoBehaviour
{
    public GameObject target; // Represent moving target
    public Vector3 targetPos; // Represent destination axis
    public float speed = 5f;
    private string side;
    private string otherside;
    private Transform[] nodes;
    private int cur = 0;
    private Animator animator;
    private float range; // Records the range of not moving, 1 for melee and 3 for caster
    private bool stop = false; // This should only be true when the enemy crystal is destroyed
    public void Init(string side, int line)
    {
        this.side = side;
        if (side.Equals("Blue"))
        {
            otherside = "Red";
        } else
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

    // Target received from combat script
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void FixedUpdate()
    {
        animator.SetBool("isWalk", false);
        // Won't move if info isn't given yet or destination reached and there are no enemies
        if (side == "" || (target == null && stop))
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
            transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
        } else
        {
            // Using destination waypoint
            transform.position += (targetPos - transform.position).normalized * speed * Time.deltaTime;
            animator.SetBool("isWalk", true);
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
