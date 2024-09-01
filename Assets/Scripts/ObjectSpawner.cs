using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private float radius = 10.0f;
    [SerializeField]
    private KeyCode key = KeyCode.F;
    [SerializeField]
    private List<GameObject> objects = new();
    

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // spawn new NPC on button press
        if (Input.GetKeyDown(key))
        {
            SpawnNPC();
        }
    }

    public void SpawnNPC()
    {
        // choose random npc
        int randomIndex = Random.Range(0, objects.Count);
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        Vector3 pos = new(Random.Range(-radius + playerX, radius + playerX), 0, Random.Range(-radius + playerZ, radius + playerZ));

        // spawn randomly within x radius of player
        GameObject newObj = Instantiate(objects[randomIndex], pos, Quaternion.identity);
    }
}