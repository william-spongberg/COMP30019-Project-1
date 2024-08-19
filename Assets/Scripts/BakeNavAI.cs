using Unity.AI.Navigation;
using UnityEngine;

public class BakeNavAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshSurface navMeshSurface;
    [SerializeField]
    private Generator generator;
    [SerializeField]
    private int objCount = 0;


    void Update()
    {
        // only bake when objects are created/destroyed

        if (objCount != generator.GetObjCount())
        {
            objCount = generator.GetObjCount();
            navMeshSurface.BuildNavMesh();
        }
    }
}