using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class EnemyNew : MonoBehaviour
{

    private enum State
    {
        Patrol,
        Tracking,
        Attacking

    }

    private State state;
    private NavMeshAgent agent;

    public Transform detectRoot;
    private AudioSource audioSource;
    public AudioClip patrolClip;
    public AudioClip trackingClip;

    public float runSpeed = 10f;
    public float patrolSpeed = 3f;
    [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float damage = 30f;
    public float attackRadius = 2f;
    private float attackDistance;

    public float fieldOfView = 50f;
    public float viewDistance = 10f;

    public LayerMask whatIsTarget;
    public LayerMask obstacleMask;

    private Transform targetTransform;
    private bool hasTarget => targetTransform != null;

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (detectRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(detectRoot.position, attackRadius);
        }

        var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
        var leftRayDirection = leftRayRotation * transform.forward;
        Handles.color = new Color(1f, 1f, 1f, 0.2f);
        Handles.DrawSolidArc(detectRoot.position, Vector3.up, leftRayDirection, fieldOfView, viewDistance);
    }

#endif

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        audioSource = GetComponent<AudioSource>();
        // skinRenderer = GetComponentInChildren<Renderer>();

        attackDistance = Vector3.Distance(transform.position,
                             new Vector3(detectRoot.position.x, transform.position.y, detectRoot.position.z)) +
                         attackRadius;

        attackDistance += agent.radius;

        agent.stoppingDistance = attackDistance;
        agent.speed = patrolSpeed;
    }

    public void Setup(float health, float damage,
            float runSpeed, float patrolSpeed, Color skinColor)
    {
        this.runSpeed = runSpeed;
        this.patrolSpeed = patrolSpeed;
    }
    void Start()
    {
        // Player = GameObject.Find("FPSController");
        StartCoroutine(UpdatePath());

    }
    void Update()
    {
        if (hasTarget) { Debug.Log(gameObject.name + "State: " + state); }
        if (GameManager.Instance.escPressed)
        {
            audioSource.Stop();
        }



        if (state == State.Tracking)
        {
            audioSource.clip = trackingClip;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

    }
    private IEnumerator UpdatePath()
    {

        while (!GameManager.Instance.isGameover)
        {

            if (hasTarget)
            {
                if (state == State.Patrol)
                {
                    state = State.Tracking;
                    agent.speed = runSpeed;

                }
                if (targetTransform != null) { agent.SetDestination(targetTransform.transform.position); }
            }
            else
            {
                if (targetTransform != null) targetTransform = null;

                if (state != State.Patrol)
                {
                    state = State.Patrol;
                    agent.speed = patrolSpeed;
                }

                if (agent.remainingDistance <= 3f)
                {
                    var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
                    agent.SetDestination(patrolPosition);
                }

                // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
                // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
                var colliders = Physics.OverlapSphere(detectRoot.position, viewDistance, whatIsTarget);

                // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
                foreach (var collider in colliders)
                {

                    Transform player = collider.transform;
                    Vector3 dirToPlayer = (player.position - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView / 2)
                    {
                        float distToPlayer = Vector3.Distance(transform.position, player.position);
                        if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                        {
                            targetTransform = player;

                        }
                    }

                }
            }


            // 0.2 초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

}