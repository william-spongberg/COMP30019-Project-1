using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject prefab;

    public float offsetX = 0.0f;
    public float offsetY = 0.0f;
    public float offsetZ = 0.0f;

    public float playerX, playerY, playerZ;

    // Start is called before the first frame update
    void Start()
    {
        prefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        playerX = player.transform.position.x;
        playerY = player.transform.position.y;
        playerZ = player.transform.position.z;

        prefab.transform.position = new Vector3(playerX + offsetX, playerY + offsetY, playerZ + offsetZ);
        prefab.transform.rotation = player.transform.rotation;
    }
}
