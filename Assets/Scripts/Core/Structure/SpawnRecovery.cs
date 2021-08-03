using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Add a 3 second buff that recovers health and add movement speed for them
public class SpawnRecovery : MonoBehaviour
{
    public string side;
    private float[] buff;
    void Start()
    {
        buff = new float[4];
        buff[3] = 2;
    }

    // Attack enemies entering base, and heal allies in base
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(side))
            {
                /*Buff spawnBuff = new Buff(0, 3, -1, buff);*/
                collision.GetComponent<HeroCombat>().BaseRecover();
            }
            else
            {
                collision.GetComponent<Combat>().GetHit(1, gameObject);
            }
        }
    }
}
