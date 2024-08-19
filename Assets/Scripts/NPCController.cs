using UnityEngine;
using UnityEngine.AI;

// TODO: jumping?
// TODO: add audio for footsteps, landing, etc
// TODO: attack player if close enough
// TODO: add animations for idle, attack, etc

// TODO: rewrite for better object-oriented practices

// need ai agent and animator
[RequireComponent (typeof (NavMeshAgent))]
[RequireComponent (typeof (Animator))]
public class NPCController : MonoBehaviour
{
    [SerializeField]
    private Vector2 smoothDeltaPosition = Vector2.zero;
    [SerializeField]
    private Vector2 velocity = Vector2.zero;

    private Animator anim;
    private NavMeshAgent agent;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        // Don’t update position automatically
        agent.updatePosition = false;
    }

    void Update()
    {
        // make nav agent move towards player
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;

        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot (transform.right, worldDeltaPosition);
        float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2 (dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
        smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

        // Update velocity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

        // Update animation parameters
        anim.SetBool("move", shouldMove);
        anim.SetFloat ("velx", velocity.x);
        anim.SetFloat ("vely", velocity.y);

        //GetComponent<LookAt>().lookAtTargetPosition = agent.steeringTarget + transform.forward;
    }
}