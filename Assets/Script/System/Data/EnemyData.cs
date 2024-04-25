using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float moveSpeed;
    public int maxHealth;
    public int damage;
    public int spawnPercent;
}
