using UnityEngine;

// General movement class define behavior for minions
public class Movement : MonoBehaviour
{
    public GameObject target; // Represent moving target
    public Vector3 targetPos; // Represent destination axis
    public float speed = 4f;
    public bool moveable = false; // Becomes true only when affected with debuffs

    // Target received from combat script
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetMoveable(bool move)
    {
        this.moveable = move;
    }
}
