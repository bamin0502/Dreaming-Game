using UnityEngine;
using UniRx;
using UnityEngine.Rendering.PostProcessing;
using EPOOutline;
using DamageNumbersPro;

public class PlayerHealth : MonoBehaviour
{
    [Tooltip("플레이어 체력 구독 변수")]
    public ReactiveProperty<int> health = new ReactiveProperty<int>(100);
    
    public Animator animator;
    public PostProcessVolume volume;
    private ColorGrading colorGrading;
    public Outlinable outlinable;
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    public DamageNumber TakeDamagePrefab;
    public Transform DamageNumberSpawnPoint;
    public bool isDead { get; private set; } = false;
    void Start()
    {
        if (volume.profile.TryGetSettings(out colorGrading))
        {
            health.Subscribe(newHealth =>
            {
                UiManager.instance.UpdateHealth(newHealth);
                Debug.Log("Health: " + newHealth);
                if (newHealth <= 0 && !isDead)
                {
                    Debug.Log("Player is dead");
                    Die();
                }
            });
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead && health.Value > 0)
        {
            health.Value -= damage;
            TakeDamagePrefab.Spawn(DamageNumberSpawnPoint.position, damage);
            health.Value=Mathf.Max(health.Value,0);
            FlashRedEffect();    
        }
        
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger(IsDead);
        outlinable.OutlineParameters.Color = Color.white;
        gameObject.GetComponent<PlayerMovement>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }

    void FlashRedEffect()
    {
        colorGrading.colorFilter.value = Color.red;
        outlinable.OutlineParameters.Color = Color.red;
        // 0.5초 후에 색상을 원래대로 복구
        Observable.Timer(System.TimeSpan.FromSeconds(0.5)).Subscribe(_ =>
        {
            colorGrading.colorFilter.value = Color.white;
            outlinable.OutlineParameters.Color = Color.green;
        }).AddTo(this);
    }
}