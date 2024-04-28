using System;
using UnityEngine;
using UnityEngine.AI;
using DamageNumbersPro;
using UniRx;
using UniRx.Triggers;

public class Boss : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private BoxCollider boxCollider;
    public  EnemyData enemyData;
    
    [Tooltip("애니메이터의 파라미터 지정")]
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int Attack1 = Animator.StringToHash("Attack1");
    private static readonly int Attack2 = Animator.StringToHash("Attack2");
    private static readonly int Target = Animator.StringToHash("Target");
    private static readonly int Relax = Animator.StringToHash("Relax");
    
    private Subject<Unit> attackTrigger = new Subject<Unit>();
    public IDisposable attackSequence;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Start()
    {
        //UniRx를 이용한 애니메이션
        this.UpdateAsObservable()
            .Select(_ => animator.GetCurrentAnimatorStateInfo(0))
            .Where(state => state.IsName("Attack1") && state.normalizedTime > 0.9f)
            .Subscribe(_ =>
            {
                animator.SetTrigger(Relax);
            });
        this.UpdateAsObservable()
            .Select(_ => animator.GetCurrentAnimatorStateInfo(0))
            .Where(state => state.IsName("Attack2") && state.normalizedTime > 0.9f)
            .Subscribe(_ =>
            {
                animator.SetTrigger(Relax);
            });

        attackSequence = attackTrigger;
        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(enemyData.damage);
        }
    }
}
