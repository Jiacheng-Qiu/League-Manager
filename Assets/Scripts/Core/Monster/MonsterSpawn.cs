using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Monsters spawn on the positions they are assigned to
public class MonsterSpawn : MonoBehaviour
{
    // Records all positions from child
    private Transform[] positions;
    private float[] spawnRecord; // Spawn time of all monsters
    private GameObject[] monsters; // Keep track of all the monsters so that will know once they dissappear
    // Spawn CD of monsters
    public float monster1Spawn = 20f;
    public float monster2Spawn = 15f;
    public float dragonSpawn = 120f;
    public float goblinSpawn = 50f;
    private GameObject spaceTaker; // This is used as a space replacer for monsters

    private void Start()
    {
        positions = new Transform[12];
        spawnRecord = new float[12];
        monsters = new GameObject[12];
        for (int i = 0; i < 12; i++)
        {
            positions[i] = transform.GetChild(i);
        }
        spaceTaker = new GameObject();
        spaceTaker.SetActive(false);
    }

    // Monsters spawn only according to names of the spawn, and CD are recorded
    private void FixedUpdate()
    {
        // Check if any monster are killed and should spawn
        for (int i = 0; i < 12; i++)
        {
            if (monsters[i] == null)
            {
                // Record the death time of monster to spawn them
                spawnRecord[i] = Time.time;
                monsters[i] = spaceTaker; // So that death time won't be refreshed during next few frames
            }
            if (CheckCD(i))
            {
                SpawnMonster(i);
            }
        }
    }

    // Check if monster on given position can be spawn again
    private bool CheckCD(int index)
    {
        // Won't spawn any living ones
        if (spawnRecord[index] == -1)
        {
            return false;
        }
        string monsterName = positions[index].name;
        float cd = 0;
        if (monsterName.Contains("1") || monsterName.Contains("2"))
        {
            cd = monster1Spawn;
        }
        else if (monsterName.Contains("3") || monsterName.Contains("4") || monsterName.Contains("5"))
        {
            cd = monster2Spawn;
        }
        else if (monsterName.Contains("Dragon"))
        {
            cd = dragonSpawn;
        } else
        {
            cd = goblinSpawn;
        }
        // Check if the monster should be spawn again
        return Time.time == 0 || Time.time >= spawnRecord[index] + cd;
    }

    // Spawn a monster of given position
    private void SpawnMonster(int index)
    {
        string monsterName = positions[index].name;
        GameObject monster = null;
        if (monsterName.Contains("1") || monsterName.Contains("2"))
        {
            monster = (GameObject)Instantiate(Resources.Load("Prefabs/Monster1"), positions[index].position, positions[index].rotation);
        }
        else if (monsterName.Contains("3") || monsterName.Contains("4") || monsterName.Contains("5"))
        {
            monster = (GameObject)Instantiate(Resources.Load("Prefabs/Monster2"), positions[index].position, positions[index].rotation);
        }
        else if (monsterName.Contains("Dragon"))
        {
            monster = (GameObject)Instantiate(Resources.Load("Prefabs/Dragon"), positions[index].position, positions[index].rotation);
        }
        else
        {
            monster = (GameObject)Instantiate(Resources.Load("Prefabs/Goblin"), positions[index].position, positions[index].rotation);
        }

        monster.GetComponent<MonsterMovement>().basePos = positions[index];
        monster.transform.parent = GameObject.Find("Monster Folder").transform;
        monsters[index] = monster;
        // Make death time be -1 to stop living monsters from continuous spawning
        spawnRecord[index] = -1;
    }
}
