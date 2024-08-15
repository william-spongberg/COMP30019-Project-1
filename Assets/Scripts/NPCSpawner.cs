using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private float radius = 10.0f;
    [SerializeField]
    private List<GameObject> npcs = new();

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // spawn new NPC on button press
        if (Input.GetKeyDown("f"))
        {
            SpawnNPC();
        }
    }

    public void SpawnNPC()
    {
        // TODO: change spawn position to be within x range of player

        // choose random npc
        int randomIndex = Random.Range(0, npcs.Count);
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        Vector3 pos = new(Random.Range(-radius + playerX, radius + playerX), 0, Random.Range(-radius + playerZ, radius + playerZ));

        // spawn randomly within x radius of player
        GameObject newObj = Instantiate(npcs[randomIndex], pos, Quaternion.identity);

        // spawn npc
        //GameObject newObj = Instantiate(npcs[randomIndex], new Vector3(0,0,0), Quaternion.identity);
    }
}