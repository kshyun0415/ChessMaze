using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Queen : MonoBehaviour
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
    public bool hasTarget => targetTransform != null;

#if UNITY_EDITOR

    private void OnDrawGizmosSelected()
    {
        if (detectRoot != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
            Gizmos.DrawSphere(detectRoot.position, viewDistance);
        }

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

        StartCoroutine(UpdatePath());

    }
    void Update()
    {
        if (hasTarget) { Debug.Log(gameObject.name + "State: " + state); }
        if (GameManager.Instance.escPressed)
        {
            audioSource.Stop();
        }
        if (GameManager.Instance.isPlayerHidden)
        {
            OnplayerHidden();
        }

        if (state == State.Patrol)
        {
            audioSource.clip = patrolClip;
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
                    audioSource.clip = trackingClip;
                    audioSource.Stop();
                    audioSource.clip = trackingClip;
                    audioSource.Play();
                    yield return new WaitForSeconds(1f);

                }
                audioSource.clip = patrolClip;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }

                GameManager.Instance.detectedByQueen = true;
                GameManager.Instance.queenTargetTransform = targetTransform;

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

                    float distToPlayer = Vector3.Distance(transform.position, player.position);
                    if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                    {
                        targetTransform = player;

                    }


                }
            }


            // 0.2 초 주기로 처리 반복
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnplayerHidden()
    {
        targetTransform = null;
        GameManager.Instance.detectedByQueen = false;
    }

}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// #if UNITY_EDITOR
// using UnityEditor;
// #endif

// public class Queen : LivingEntity
// {
//     private enum State
//     {
//         Patrol,
//         Tracking,
//         Attacking
//     }

//     private State state;

//     private NavMeshAgent agent; // 경로계산 AI 에이전트
//     private Animator animator; // 애니메이터 컴포넌트

//     public Transform detectRoot;
//     public Transform eyeTransform;

//     private AudioSource audioSource; // 오디오 소스 컴포넌트

//     public AudioClip patrolClip;
//     public AudioClip laughingClip; // 추격시 재생할 소리

//     // private Renderer skinRenderer; // 렌더러 컴포넌트

//     public float runSpeed = 10f;
//     [Range(0.01f, 2f)] public float turnSmoothTime = 0.1f;
//     private float turnSmoothVelocity;

//     public float damage = 30f;
//     public float detectRadius = 2f;
//     private float attackDistance;

//     public float fieldOfView = 50f;
//     public float detectDistance = 10f;
//     public float patrolSpeed = 3f;

//     [HideInInspector] public LivingEntity targetEntity; // 추적할 대상
//     public LayerMask whatIsTarget; // 추적 대상 레이어

//     private GameObject Player;

//     private RaycastHit[] hits = new RaycastHit[10];
//     private List<LivingEntity> lastAttackedTargets = new List<LivingEntity>();

//     // private bool hasTarget => targetEntity != null && !targetEntity.dead;

//     private bool hasTarget => targetEntity != null && !targetEntity.dead;



// #if UNITY_EDITOR

//     private void OnDrawGizmosSelected()
//     {
//         if (detectRoot != null)
//         {
//             Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
//             Gizmos.DrawSphere(detectRoot.position, detectDistance);
//         }

//         // var leftRayRotation = Quaternion.AngleAxis(-fieldOfView * 0.5f, Vector3.up);
//         // var leftRayDirection = leftRayRotation * transform.forward;
//         // Handles.color = new Color(1f, 1f, 1f, 0.2f);
//         // Handles.DrawSolidArc(eyeTransform.position, Vector3.up, leftRayDirection, fieldOfView, detectDistance);
//     }

// #endif

//     private void Awake()
//     {
//         agent = GetComponent<NavMeshAgent>();
//         animator = GetComponent<Animator>();
//         audioSource = GetComponent<AudioSource>();
//         // skinRenderer = GetComponentInChildren<Renderer>();

//         attackDistance = Vector3.Distance(transform.position,
//                              new Vector3(detectRoot.position.x, transform.position.y, detectRoot.position.z)) +
//                          detectRadius;

//         attackDistance += agent.radius;

//         agent.stoppingDistance = attackDistance;
//         agent.speed = patrolSpeed;
//     }
//     public void Setup(float health, float damage,
//             float runSpeed, float patrolSpeed, Color skinColor)
//     {
//         // 체력 설정
//         this.startingHealth = health;
//         this.health = health;

//         // 내비메쉬 에이전트의 이동 속도 설정
//         this.runSpeed = runSpeed;
//         this.patrolSpeed = patrolSpeed;

//         this.damage = damage;

//         // 렌더러가 사용중인 머테리얼의 컬러를 변경, 외형 색이 변함
//         // skinRenderer.material.color = skinColor;
//     }

//     void Start()
//     {
//         Player = GameObject.Find("FPSController");
//         StartCoroutine(UpdatePath());

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (GameManager.Instance.escPressed)
//         {
//             audioSource.Stop();
//         }
//         // Debug.Log(gameObject.name + "State: " + state);
//         if (dead) return;



//         if (GameManager.Instance.isPlayerHidden == true)
//         {
//             OnPlayerHidden();
//         }
//         else
//         {
//             OnPlayerNotHidden();
//         }

//         if (state == State.Patrol)
//         {
//             audioSource.clip = patrolClip;
//             if (!audioSource.isPlaying)
//             {
//                 audioSource.Play();
//             }
//         }
//         if (state == State.Tracking)
//         {
//             audioSource.clip = laughingClip;
//             if (!audioSource.isPlaying)
//             {
//                 audioSource.Play();

//             }
//         }



//     }

//     private void FixedUpdate()
//     {
//         if (dead) return;


//         if (state == State.Attacking)
//         {
//             var lookRotation =
//                 Quaternion.LookRotation(targetEntity.transform.position - transform.position, Vector3.up);
//             var targetAngleY = lookRotation.eulerAngles.y;

//             transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngleY,
//                                         ref turnSmoothVelocity, turnSmoothTime);
//         }

//         if (state == State.Attacking)
//         {
//             var direction = transform.forward;
//             var deltaDistance = agent.velocity.magnitude * Time.deltaTime;

//             var size = Physics.SphereCastNonAlloc(detectRoot.position, detectRadius, direction, hits, deltaDistance,
//                 whatIsTarget);

//             for (var i = 0; i < size; i++)
//             {
//                 var attackTargetEntity = hits[i].collider.GetComponent<LivingEntity>();

//                 if (attackTargetEntity != null && !lastAttackedTargets.Contains(attackTargetEntity))
//                 {
//                     var message = new DamageMessage();
//                     message.amount = damage;
//                     message.damager = gameObject;
//                     message.hitPoint = detectRoot.TransformPoint(hits[i].point);
//                     message.hitNormal = detectRoot.TransformDirection(hits[i].normal);

//                     attackTargetEntity.ApplyDamage(message);

//                     lastAttackedTargets.Add(attackTargetEntity);
//                     break;
//                 }
//             }
//         }
//     }
//     private IEnumerator UpdatePath()
//     {
//         // 살아있는 동안 무한 루프
//         while (!dead)
//         {
//             if (hasTarget)
//             {

//                 if (state == State.Patrol)
//                 {
//                     state = State.Tracking;
//                     // agent.speed = runSpeed;

//                 }
//                 Debug.Log(gameObject.name + " " + state);

//                 GameManager.Instance.detectedByQueen = true;
//                 GameManager.Instance.playerTransform = targetEntity.transform;
//                 yield return new WaitForSeconds(2f);
//                 if (agent.remainingDistance <= 3f)
//                 {
//                     var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
//                     agent.SetDestination(patrolPosition);
//                 }


//                 // 추적 대상 존재 : 경로를 갱신하고 AI 이동을 계속 진행
//             }
//             else
//             {
//                 GameManager.Instance.detectedByQueen = false;


//                 if (targetEntity != null) targetEntity = null;

//                 if (state != State.Patrol)
//                 {
//                     state = State.Patrol;
//                     agent.speed = patrolSpeed;

//                 }

//                 if (agent.remainingDistance <= 3f)
//                 {
//                     var patrolPosition = Utility.GetRandomPointOnNavMesh(transform.position, 20f, NavMesh.AllAreas);
//                     agent.SetDestination(patrolPosition);
//                 }

//                 // 20 유닛의 반지름을 가진 가상의 구를 그렸을때, 구와 겹치는 모든 콜라이더를 가져옴
//                 // 단, whatIsTarget 레이어를 가진 콜라이더만 가져오도록 필터링
//                 var colliders = Physics.OverlapSphere(eyeTransform.position, detectDistance, whatIsTarget);

//                 // 모든 콜라이더들을 순회하면서, 살아있는 LivingEntity 찾기
//                 foreach (var collider in colliders)
//                 {
//                     // if (!IsTargetOnSight(collider.transform)) break;

//                     var livingEntity = collider.GetComponent<LivingEntity>();

//                     // LivingEntity 컴포넌트가 존재하며, 해당 LivingEntity가 살아있다면,
//                     if (livingEntity != null && !livingEntity.dead)
//                     {
//                         // 추적 대상을 해당 LivingEntity로 설정
//                         targetEntity = livingEntity;
//                         // audioSource.clip = laughingClip;
//                         // if (!audioSource.isPlaying)
//                         // {
//                         //     audioSource.Play();

//                         // }
//                         // state = State.Tracking;

//                         // for문 루프 즉시 정지
//                         break;
//                     }
//                     else
//                     {
//                         state = State.Patrol;
//                     }
//                 }
//             }

//             // 0.2 초 주기로 처리 반복
//             yield return new WaitForSeconds(0.2f);
//         }
//     }

//     private bool IsTargetOnSight(Transform target)
//     {
//         RaycastHit hit;

//         var direction = target.position - eyeTransform.position;

//         direction.y = eyeTransform.forward.y;

//         if (Vector3.Angle(direction, eyeTransform.forward) > fieldOfView * 0.5f)
//         {
//             return false;
//         }

//         if (Physics.Raycast(eyeTransform.position, direction, out hit, detectDistance, whatIsTarget))
//         {
//             if (hit.transform == target) return true;
//         }

//         return false;
//     }

//     public void OnPlayerHidden()
//     {
//         Player.layer = 7;
//         targetEntity = null;
//     }
//     public void OnPlayerNotHidden()
//     {
//         Player.layer = 3;
//     }

// }