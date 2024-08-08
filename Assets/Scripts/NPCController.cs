using UnityEngine;
using UnityEngine.AI;

// TODO: worthwhile? may use external A* asset instead https://arongranberg.com/astar/features

// TODO: jumping?
// TODO: add audio for footsteps, landing, etc
// TODO: attack player if close enough
// TODO: add animations for idle, attack, etc

// TODO: rewrite for better object-oriented practices

public class NPCController : MonoBehaviour {
    public float moveSpeed = 3.5f;
    public float turnSpeed = 120f;

    public Animator animator;
    public NavMeshAgent agent;
    public GameObject player;
    public Vector3 destination;

    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    private bool isGrounded;
    private float _animationBlend;
    private float _speed;
    private float _targetRotation;
    private float _rotationVelocity;
    private float _verticalVelocity;
    
    private const float SpeedChangeRate = 10.0f;
    private const float RotationSmoothTime = 0.12f;

    void Start() {
        if (agent == null) {
            agent = GetComponent<NavMeshAgent>();
        }
        if (animator == null) {
            animator = GetComponent<Animator>();
        }

        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

        // make sure on NavMesh
        if (agent != null && agent.isOnNavMesh) {
            agent.speed = moveSpeed;
        } else {
            Debug.LogError("NavMeshAgent is not placed on a NavMesh.");
        }
    }

    void Update() {
        destination = player.transform.position;

        // update destination
        if (agent != null && agent.isOnNavMesh) {
            agent.SetDestination(destination);

            // update animations based on velocity
            Vector3 velocity = agent.velocity;
            float speed = velocity.magnitude;
            animator.SetFloat(_animIDSpeed, speed);

            // rotate towards destination
            if (velocity != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(velocity);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            // check if grounded
            isGrounded = IsGrounded();
            animator.SetBool(_animIDGrounded, isGrounded);

            // check if in free fall
            if (!isGrounded && !agent.isOnOffMeshLink) {
                animator.SetBool(_animIDFreeFall, true);
            } else {
                animator.SetBool(_animIDFreeFall, false);
            }
        }

        Move();
    }

    private void Move() {
        float targetSpeed = moveSpeed;
        float currentHorizontalSpeed = new Vector3(agent.velocity.x, 0.0f, agent.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = 1f;

        // accelerate/decelerate to target speed
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset) {
            // non-linear speed change
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            // round to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        } else {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // calculate input direction based on destination
        Vector3 inputDirection = (destination - transform.position).normalized;

        // rotate to face destination
        if (inputDirection != Vector3.zero) {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        // move NPC
        agent.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        // update animator
        if (animator != null) {
            animator.SetFloat(_animIDSpeed, _animationBlend);
            animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        // apply gravity
        if (!isGrounded) {
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        } else if (_verticalVelocity < 0) {
            _verticalVelocity = 0;
        }
    }

    private bool IsGrounded() {
        // raycast to check if grounded
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        return Physics.Raycast(ray, 0.2f);
    }

    public void SetDestination(Vector3 newDestination) {
        if (agent != null && agent.isOnNavMesh) {
            destination = newDestination;
        } else {
            Debug.LogError("ERROR: can't set destination. NavMeshAgent not on NavMesh.");
        }
    }

    public void Stop() {
        if (agent != null && agent.isOnNavMesh) {
            agent.isStopped = true;
            animator.SetFloat(_animIDSpeed, 0);
        }
    }

    public void Resume() {
        if (agent != null && agent.isOnNavMesh) {
            agent.isStopped = false;
        }
    }

    // commented out audio for now
    private void OnFootstep(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            //// if (FootstepAudioClips.Length > 0) {
            //     //var index = Random.Range(0, FootstepAudioClips.Length);
            //     //AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            //// }
        }
    }

    private void OnLand(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            ////AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
    }
}