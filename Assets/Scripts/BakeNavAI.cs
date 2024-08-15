using Unity.AI.Navigation;
using UnityEngine;

public class BakeNavAI : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    public Generator generator;

    public int objCount = 0;


    void Update()
    {
        // only bake when objects are created/destroyed

        if (objCount != generator.GetObjCount()) {
            objCount = generator.GetObjCount();
            navMeshSurface.BuildNavMesh();
        }
    }
}