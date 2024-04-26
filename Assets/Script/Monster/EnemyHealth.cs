using UnityEngine;
using UniRx;
using UnityEngine.AI;
using DamageNumbersPro;


public class EnemyHealth : MonoBehaviour
{
    public int health;
    private EnemyData enemyData;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private BoxCollider boxCollider;

    
    public DamageNumber EnemyTakeDamage;
    public Transform damageNumberSpawnPoint;
    
    [Tooltip("애니메이터의 파라미터 지정")]
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public void Awake()
    {
        enemyData = GetComponent<Enemy>().enemyData;
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();
        
    }
    
    public void Start()
    {
        health = enemyData.maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        EnemyTakeDamage.Spawn(damageNumberSpawnPoint.position, damage);
        if (health <= 0)
        {
            Die();
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
            boxCollider.enabled = false;

            GameManager.instance.AddScore(enemyData.score);
        }
    }

    private void Die()
    {
        animator.SetTrigger(IsDead);
        
        Observable.Timer(System.TimeSpan.FromSeconds(5))
            .Subscribe(_ =>
            {
                gameObject.SetActive(false);
                
            });
    }
}
