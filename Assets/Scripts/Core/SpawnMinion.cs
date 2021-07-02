using System;
using UnityEngine;

// Crystal spawn minions every few seconds
public class SpawnMinion : MonoBehaviour
{
    public float spawnCD = 20;
    private float lastSpawn = -20; // Make minions spawn after 15s of game start
    public string side = "Blue";
    private bool spawnCaster = false;
    private Transform spawn0;
    private Transform spawn1;
    private void Start()
    {
        spawn0 = GameObject.Find("Minion Path").transform.Find(side + " Spawn0");
        spawn1 = GameObject.Find("Minion Path").transform.Find(side + " Spawn1");
    }
    private void FixedUpdate()
    {
        if (spawnCaster && Time.time >= lastSpawn + 1)
        {
            SpawnCaster();
        }
        if (Time.time >= spawnCD + lastSpawn)
        {
            SpawnMelee();
            lastSpawn = Time.time;
        }
    }

    // Casters are spawned 1 second after melee
    private void SpawnCaster()
    {
        ((GameObject)Instantiate(Resources.Load("Prefabs/" + side + " Caster"), spawn0.position, spawn0.rotation)).GetComponent<Movement>().Init(side, 0);
        ((GameObject)Instantiate(Resources.Load("Prefabs/" + side + " Caster"), spawn1.position, spawn1.rotation)).GetComponent<Movement>().Init(side, 1);
        spawnCaster = false;
    }

    // Two melees are spawned at position
    private void SpawnMelee()
    {
        ((GameObject)Instantiate(Resources.Load("Prefabs/" + side + " Melee"), spawn0.position, spawn0.rotation)).GetComponent<Movement>().Init(side, 0);
        ((GameObject)Instantiate(Resources.Load("Prefabs/" + side + " Melee"), spawn1.position, spawn1.rotation)).GetComponent<Movement>().Init(side, 1);
        spawnCaster = true;
    }
}
