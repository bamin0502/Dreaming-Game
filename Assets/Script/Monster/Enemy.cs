using UniRx;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform target;
    public EnemyData enemyData;
    private Animator animator;
    private static readonly int IsTarget = Animator.StringToHash("IsTarget");

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    public void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                if (target != null && navMeshAgent != null && navMeshAgent.enabled && gameObject.activeInHierarchy)
                {
                    navMeshAgent.SetDestination(target.position);
                    navMeshAgent.speed=enemyData.moveSpeed;
                    animator.SetBool(IsTarget, true);
                }
                else
                {
                    Debug.LogWarning("Target or NavMeshAgent is null");
                }
            })
            .AddTo(this);
    }
    public void Update()
    {
        
    }
    
}
