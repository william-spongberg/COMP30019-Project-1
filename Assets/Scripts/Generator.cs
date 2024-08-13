using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Generator : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> objectPrefabs = new List<GameObject>();
    public List<Vector3> objectOffsets = new List<Vector3>();
    public List<Vector3> objectDimensions = new List<Vector3>();

    public int radius = 10;
    public float objectGap = 0f;

    private float objectWidth, objectHeight, objectLength;

    public Dictionary<Vector2Int, GameObject> objects = new Dictionary<Vector2Int, GameObject>();
    
    void Start()
    {
        // set object offsets
        for (int i = 0; i < objectPrefabs.Count; i++) {
            objectOffsets.Add(new Vector3(0, 0, 0));
        }

        // get object dimensions
        for (int i = 0; i < objectPrefabs.Count; i++) {
            Vector3 objectDimension = objectPrefabs[i].GetComponent<Renderer>().bounds.size;
            objectDimensions.Add(objectDimension);
        }


        // TODO: need to all be same length, fix in future?
        objectWidth = objectDimensions[0].x;
        objectHeight = objectDimensions[0].y;
        objectLength = objectDimensions[0].z;
    }

    void Update()
    {
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;
        
        CreateObjects(playerX, playerZ);
        DestroyObjects(playerX, playerZ);
    }

    void CreateObjects(float playerX, float playerZ) {
        int startX = Mathf.FloorToInt((playerX - radius) / (objectWidth + objectGap));
        int endX = Mathf.CeilToInt((playerX + radius) / (objectWidth + objectGap));
        int startY = Mathf.FloorToInt((playerZ - radius) / (objectLength + objectGap));
        int endY = Mathf.CeilToInt((playerZ + radius) / (objectLength + objectGap));
    
        for (int x = startX; x <= endX; x++) {
            for (int y = startY; y <= endY; y++) {
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (!objects.ContainsKey(gridPosition)) {
                    Vector3 worldPosition = new Vector3(x * (objectWidth + objectGap), -objectHeight / 2.0f, y * (objectLength + objectGap));

                    int randomIndex = Random.Range(0, objectPrefabs.Count);
                    GameObject objectPrefab = objectPrefabs[randomIndex];
                    Vector3 offset = objectOffsets[randomIndex];
                    objectPrefab = Instantiate(objectPrefab, worldPosition + offset, Quaternion.identity);
                    objects[gridPosition] = objectPrefab;
                }
            }
        }
    }

    void DestroyObjects(float playerX, float playerZ) {
        int startX = Mathf.FloorToInt((playerX - radius) / objectWidth + objectGap);
        int endX = Mathf.CeilToInt((playerX + radius) / objectWidth + objectGap);
        int startY = Mathf.FloorToInt((playerZ - radius) / objectLength + objectGap);
        int endY = Mathf.CeilToInt((playerZ + radius) / objectLength + objectGap);

        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in objects) {
            Vector2Int gridPosition = kvp.Key;
            if (gridPosition.x < startX || gridPosition.x > endX || gridPosition.y < startY || gridPosition.y > endY) {
                Destroy(kvp.Value);
                keysToRemove.Add(gridPosition);
            }
        }

        foreach (var key in keysToRemove) {
            objects.Remove(key);
        }
    }
}