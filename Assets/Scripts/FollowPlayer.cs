using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private Vector3 offsetVector = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 playerCoords;
    //[SerializeField]
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        prefab = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        playerCoords.x = player.transform.position.x;
        playerCoords.y = player.transform.position.y;
        playerCoords.z = player.transform.position.z;

        prefab.transform.position = playerCoords + offsetVector;
        prefab.transform.rotation = player.transform.rotation;
    }
}
