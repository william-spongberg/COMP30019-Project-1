using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

// need ai agent and animator
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class NPCController : MonoBehaviour
{
    [SerializeField]
    private float speed = 0.0f;
    [SerializeField]
    private Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 dest = Vector3.zero;

    private Animator anim;
    private NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        // Don’t update position automatically
        //agent.updatePosition = false;
    }

    void Update()
    {
        // make nav agent move towards player
        agent.destination = player.transform.position;
        dest = agent.destination;
        velocity = agent.velocity;
        speed = agent.velocity.magnitude;
        
        // let animation know agent speed
        anim.SetFloat("Speed", speed);

        // animation multiplier
        anim.SetFloat("MotionSpeed", speed);
    }
}