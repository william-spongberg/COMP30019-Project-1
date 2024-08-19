using System.Collections.Generic;
using UnityEngine;

// TODO: implement Perlin noise for choosing tiles
// * also avoids terrain previously explored being completely deleted
// * could also use to spawn enemies?? use different kinds of noise for different generation
// * can still be a new game everytime (like minecraft)

[System.Serializable]


public class SpawnObject
{
    public GameObject obj = null;
    public Vector3 dimensions = new(0, 0, 0);
    public Vector3 offsets = new(0, 0, 0);
    public int spawnWeight = 10;
}

public class Generator : MonoBehaviour
{
    [SerializeField]
    private int radius = 10;
    [SerializeField]
    private float gap = 0f;
    [SerializeField]
    private Vector3 gridDimensions = new(0, 0, 0);
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private List<SpawnObject> spawnObjects = new List<SpawnObject>();
    [SerializeField]
    private Dictionary<Vector2Int, GameObject> objects = new Dictionary<Vector2Int, GameObject>();

    void Start()
    {
        // update dimensions for each spawn object if not manually set
        for (int i = 0; i < spawnObjects.Count; i++)
        {
            if (spawnObjects[i].dimensions != null)
                spawnObjects[i].dimensions = spawnObjects[i].obj.GetComponent<Renderer>().bounds.size;
        }

        // TODO: need to all be same length, fix in future?
        // ! for now just set grid size to size of first object in list
        gridDimensions.x = spawnObjects[0].dimensions.x;
        gridDimensions.y = spawnObjects[0].dimensions.y;
        gridDimensions.z = spawnObjects[0].dimensions.z;
    }

    void Update()
    {
        // get player 2d coords
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        // generate world
        WorldGeneration(playerX, playerZ);
    }

    void WorldGeneration(float playerX, float playerZ)
    {
        CreateObjects(playerX, playerZ);
        DestroyObjects(playerX, playerZ);
    }

    void CreateObjects(float playerX, float playerZ)
    {
        int startX = Mathf.FloorToInt((playerX - radius) / (gridDimensions.x + gap));
        int endX = Mathf.CeilToInt((playerX + radius) / (gridDimensions.x + gap));
        int startY = Mathf.FloorToInt((playerZ - radius) / (gridDimensions.z + gap));
        int endY = Mathf.CeilToInt((playerZ + radius) / (gridDimensions.z + gap));

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (!objects.ContainsKey(gridPosition))
                {
                    Vector3 worldPosition = new Vector3(x * (gridDimensions.x + gap), -gridDimensions.y, y * (gridDimensions.z + gap));

                    SpawnObject sObj = GetRandomObj();
                    Vector3 offset = sObj.offsets;
                    GameObject obj = sObj.obj;

                    obj = Instantiate(obj, worldPosition + offset, Quaternion.identity);
                    objects[gridPosition] = obj;
                }
            }
        }
    }

    void DestroyObjects(float playerX, float playerZ)
    {
        int startX = Mathf.FloorToInt((playerX - radius) / gridDimensions.x + gap);
        int endX = Mathf.CeilToInt((playerX + radius) / gridDimensions.x + gap);
        int startY = Mathf.FloorToInt((playerZ - radius) / gridDimensions.z + gap);
        int endY = Mathf.CeilToInt((playerZ + radius) / gridDimensions.z + gap);

        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in objects)
        {
            Vector2Int gridPosition = kvp.Key;
            if (gridPosition.x < startX || gridPosition.x > endX || gridPosition.y < startY || gridPosition.y > endY)
            {
                Destroy(kvp.Value);
                keysToRemove.Add(gridPosition);
            }
        }

        foreach (var key in keysToRemove)
        {
            objects.Remove(key);
        }
    }

    public int GetObjCount()
    {
        return objects.Count;
    }

    SpawnObject GetRandomObj() {
        int totalWeight = 0;
        int currentWeight = 0;

        // sum all weights
        foreach(SpawnObject obj in spawnObjects) {
            totalWeight += obj.spawnWeight;
        }

        // pick random weight
        int randomWeight = Random.Range(0, totalWeight);
        
        foreach(SpawnObject obj in spawnObjects) {
            currentWeight += obj.spawnWeight;
            if (randomWeight <= currentWeight) {
                return obj;
            }
        }
        return null;
    }
}