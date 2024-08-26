using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnObject
{
    public GameObject obj = null;
    public Vector3 dimensions = new(0, 0, 0);
    public Vector3 offsets = new(0, 0, 0);
    // public int spawnWeight = 10;
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

    // perlin noise vars
    [SerializeField]
    private float noiseScale = 20f;
    [SerializeField]
    private float offsetX = 100f;
    [SerializeField]
    private float offsetY = 100f;

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
        // * for now just set grid size to size of first object in list
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
                    float noiseValue = GetPerlinNoiseValue(x, y, noiseScale, offsetX, offsetY);
                    SpawnObject sObj = GetObjectBasedOnNoise(noiseValue);
                    if (sObj != null)
                    {

                        Vector3 worldPosition = new Vector3(x * (gridDimensions.x + gap), -gridDimensions.y, y * (gridDimensions.z + gap));
                        Vector3 offset = sObj.offsets;
                        GameObject obj = sObj.obj;

                        obj = Instantiate(obj, worldPosition + offset, Quaternion.identity);

                        // ! in progress attempt to fix objects not being of same size in grid
                        // scale to grid scale if not the same
                        if (obj.GetComponent<Renderer>().bounds.size.x != gridDimensions.x)
                        {
                            float scale = gridDimensions.x / obj.GetComponent<Renderer>().bounds.size.x;
                            obj.transform.localScale = new Vector3(scale, scale, scale);
                        }
                        else if (obj.GetComponent<Renderer>().bounds.size.z != gridDimensions.z)
                        {
                            float scale = gridDimensions.z / obj.GetComponent<Renderer>().bounds.size.z;
                            obj.transform.localScale = new Vector3(scale, scale, scale);
                        }

                        objects[gridPosition] = obj;
                    }
                }
            }
        }
    }

    float GetPerlinNoiseValue(int x, int y, float scale, float offsetX, float offsetY)
    {
        // built-in perlin noise
        float xCoord = (float)x / scale + offsetX;
        float yCoord = (float)y / scale + offsetY;
        return Mathf.PerlinNoise(xCoord, yCoord);
    }

    SpawnObject GetObjectBasedOnNoise(float noiseValue)
    {
        // map noise to a spawn object based on object count
        int index = Mathf.FloorToInt(noiseValue * spawnObjects.Count);
        index = Mathf.Clamp(index, 0, spawnObjects.Count - 1);
        return spawnObjects[index];
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

    // old spawn logic, purely random
    /*
    SpawnObject GetRandomObj()
    {
        int totalWeight = 0;
        int currentWeight = 0;

        // sum all weights
        foreach (SpawnObject obj in spawnObjects)
        {
            totalWeight += obj.spawnWeight;
        }

        // pick random weight
        int randomWeight = Random.Range(0, totalWeight);

        foreach (SpawnObject obj in spawnObjects)
        {
            currentWeight += obj.spawnWeight;
            if (randomWeight <= currentWeight)
            {
                return obj;
            }
        }
        return null;
    } */
}