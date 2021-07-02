using UnityEngine;

// Script designed to make crystals move up and down
public class CrystalBreath : MonoBehaviour
{
    private float current = 0;
    public float speed = 0.1f;
    private bool up = true;
    public float uperLimit = 1;
    private float origYPos;
    private void Start()
    {
        origYPos = transform.position.y;
    }

    // Changing the position of the crystal while making the position of collider stay the same
    private void FixedUpdate()
    {
        if (current >= uperLimit)
        {
            up = false;
        } else  if (current <= 0)
        {
            up = true;
        }
        current += (up) ? speed : -speed;
        transform.position = new Vector3(transform.position.x, origYPos + current, transform.position.z);
        gameObject.GetComponent<CapsuleCollider2D>().offset = new Vector2(0, -current);
    }
}
