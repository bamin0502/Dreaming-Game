using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    private int damage;
    
    private void Start()
    {
        damage = GetComponent<Enemy>().enemyData.damage;   
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
        }
    }

}
