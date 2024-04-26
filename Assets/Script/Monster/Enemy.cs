using UniRx;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform target;
    public EnemyData enemyData;
    private Animator animator;
    private static readonly int IsTarget = Animator.StringToHash("IsTarget");
    private PlayerHealth playerHealth;
    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = target.GetComponent<PlayerHealth>();
        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (target != null && navMeshAgent != null && navMeshAgent.enabled && !playerHealth.isDead )
                {
                    navMeshAgent.SetDestination(target.position);
                    navMeshAgent.speed=enemyData.moveSpeed;
                    navMeshAgent.isStopped = false;
                    animator.SetBool(IsTarget, true);
                }
                else
                {
                    Debug.LogWarning("Target or NavMeshAgent is null");
                    animator.SetBool(IsTarget, false);
                    
                }
                
                if(target==null)
                {
                    navMeshAgent.isStopped = true;
                    animator.SetBool(IsTarget, false);
                }
            })
            .AddTo(this);
    }
    public void Update()
    {
        
    }
    
}
