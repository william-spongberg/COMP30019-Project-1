 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// make grid pattern of width/length of given floor prefab
// fill radius to prefab size etc, avoids overlapping gameobjects
// make 2d array radius*radius (change floors list to 2d array)

// can't change radius size dynamically now if using array? potentially use 2d list instead to keep dynamic

// how to add corridors? after every 2 desks in x direction swap for corridor prefab? (every 3rd object in x dir is a corridor)

// TODO: fix walls not destroying properly
// TODO: decide if walls worth having? fog covers outside anyway

// TODO: implement dynamic nav mesh to allow usage of built-in pathfinding
// TODO: seperate walls/floors into classes, implement object-oriented coding practices for better reusability

// TODO: randomly generate desks, chairs, etc (how much should they change?) (randomize rotation, position, etc but very slightly within bounds)

public class FloorGenerator : MonoBehaviour
{
    public GameObject player;
    public GameObject floorPrefab;
    // public GameObject wallPrefab;

    ////public NavMeshSurface navMeshSurface;

    public int radius = 10;

    public float floorGap = 0f;

    public float floorWidth = 10.0f;
    public float floorLength = 10.0f;
    public float floorHeight = 0.0f;

    public float wallWidth = 10.0f;
    public float wallLength = 1.0f;

    public float wallHeight = 10.0f;

    private Dictionary<Vector2Int, GameObject> floors = new Dictionary<Vector2Int, GameObject>();
    //private Dictionary<Vector2Int, GameObject> walls = new Dictionary<Vector2Int, GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        floorWidth = floorPrefab.GetComponent<MeshRenderer>().bounds.size.x;
        floorLength = floorPrefab.GetComponent<MeshRenderer>().bounds.size.z;
        floorHeight = floorPrefab.GetComponent<MeshRenderer>().bounds.size.y;

        // wallWidth = wallPrefab.GetComponent<MeshRenderer>().bounds.size.x;
        // wallLength = wallPrefab.GetComponent<MeshRenderer>().bounds.size.z;
        // wallHeight = wallPrefab.GetComponent<MeshRenderer>().bounds.size.y;

        ////navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        ////Debug.Log("Player position is: "+ player.transform.position);
        float playerX = player.transform.position.x;
        float playerZ = player.transform.position.z;

        // get length of walls need
        
        CreateFloors(playerX, playerZ);
        //CreateWalls(playerX, playerZ);

        ////Debug.Log("Number of floors after creation: "+ floors.Count);
        
        DestroyFloors(playerX, playerZ);
        //DestroyWalls(playerX, playerZ);

        ////Debug.Log("Number of floors after destruction: "+ floors.Count);
    }

    void CreateFloors(float playerX, float playerZ) {
        // calculate bounds
        int startX = Mathf.FloorToInt((playerX - radius) / (floorWidth + floorGap));
        int endX = Mathf.CeilToInt((playerX + radius) / (floorWidth + floorGap));
        int startY = Mathf.FloorToInt((playerZ - radius) / (floorLength + floorGap));
        int endY = Mathf.CeilToInt((playerZ + radius) / (floorLength + floorGap));
    
        // create floors within radius
        for (int x = startX; x <= endX; x++) {
            for (int y = startY; y <= endY; y++) {
                Vector2Int gridPosition = new Vector2Int(x, y);
                if (!floors.ContainsKey(gridPosition)) {
                    Vector3 worldPosition = new Vector3(x * (floorWidth + floorGap), -floorHeight / 2.0f, y * (floorLength + floorGap));
                    GameObject floor = Instantiate(floorPrefab, worldPosition, Quaternion.identity);
                    floors[gridPosition] = floor;
                }
            }
        }
    }

    void DestroyFloors(float playerX, float playerZ) {
        // calculate bounds
        int startX = Mathf.FloorToInt((playerX - radius) / floorWidth + floorGap);
        int endX = Mathf.CeilToInt((playerX + radius) / floorWidth + floorGap);
        int startY = Mathf.FloorToInt((playerZ - radius) / floorLength + floorGap);
        int endY = Mathf.CeilToInt((playerZ + radius) / floorLength + floorGap);

        List<Vector2Int> keysToRemove = new List<Vector2Int>();

        foreach (var kvp in floors) {
            Vector2Int gridPosition = kvp.Key;
            if (gridPosition.x < startX || gridPosition.x > endX || gridPosition.y < startY || gridPosition.y > endY) {
                Destroy(kvp.Value);
                keysToRemove.Add(gridPosition);
            }
        }

        foreach (var key in keysToRemove) {
            floors.Remove(key);
        }
    }

    // void CreateWalls(float playerX, float playerZ) {
    //     // calculate bounds
    //     int startX = Mathf.FloorToInt((playerX - radius) / (floorWidth + floorGap));
    //     int endX = Mathf.CeilToInt((playerX + radius) / (floorWidth + floorGap));
    //     int startY = Mathf.FloorToInt((playerZ - radius) / (floorLength + floorGap));
    //     int endY = Mathf.CeilToInt((playerZ + radius) / (floorLength + floorGap));
    //
    //     // create walls within radius
    //     for (int x = startX - 1; x <= endX + 1; x++) {
    //         CreateWall(new Vector2Int(x, startY - 1));
    //         CreateWall(new Vector2Int(x, endY + 1));
    //     }
    //
    //     for (int y = startY - 1; y <= endY + 1; y++) {
    //         CreateWallRotated(new Vector2Int(startX - 1, y));
    //         CreateWallRotated(new Vector2Int(endX + 1, y));
    //     }
    // }
    //
    // void CreateWall(Vector2Int gridPosition) {
    //     if (!walls.ContainsKey(gridPosition)) {
    //         Vector3 worldPosition = new Vector3(gridPosition.x * (floorWidth + floorGap), 0, gridPosition.y * (floorLength + floorGap));
    //         GameObject wall = Instantiate(wallPrefab, worldPosition, Quaternion.identity);
    //         walls[gridPosition] = wall;
    //     }
    // }
    //
    // void CreateWallRotated(Vector2Int gridPosition) {
    //     if (!walls.ContainsKey(gridPosition)) {
    //         Vector3 worldPosition = new Vector3(gridPosition.x * (floorWidth + floorGap), 0, gridPosition.y * (floorLength + floorGap));
    //         GameObject wall = Instantiate(wallPrefab, worldPosition, Quaternion.Euler(0, 90, 0));
    //         walls[gridPosition] = wall;
    //     }
    // }
    //
    // void DestroyWalls(float playerX, float playerZ) {
    //     // calculate bounds for wall destruction
    //     int startX = Mathf.FloorToInt((playerX - radius) / (floorWidth + floorGap)) - 1;
    //     int endX = Mathf.CeilToInt((playerX + radius) / (floorWidth + floorGap)) + 1;
    //     int startY = Mathf.FloorToInt((playerZ - radius) / (floorLength + floorGap)) - 1;
    //     int endY = Mathf.CeilToInt((playerZ + radius) / (floorLength + floorGap)) + 1;
    //
    //     List<Vector2Int> keysToRemove = new List<Vector2Int>();
    //
    //     // if player gets too close or too far away destroy
    //     foreach (var kvp in walls) {
    //         Vector2Int gridPosition = kvp.Key;
    //         if (gridPosition.x < startX || gridPosition.x > endX || gridPosition.y < startY || gridPosition.y > endY ||
    //             Mathf.Abs(gridPosition.x - playerX) > radius || Mathf.Abs(gridPosition.y - playerZ) > radius) {
    //             Destroy(kvp.Value);
    //             keysToRemove.Add(gridPosition);
    //         }
    //     }
    //
    //     foreach (var key in keysToRemove) {
    //         walls.Remove(key);
    //     }
    // }
}
