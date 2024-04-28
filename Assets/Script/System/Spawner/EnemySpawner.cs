using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public float spawnInterval = 1.0f;
    private List<GameObject> enemyPool = new List<GameObject>();
    private PlayerHealth playerHealth;
    private EnemyData enemyData;
    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        Observable.Interval(System.TimeSpan.FromSeconds(spawnInterval))
            .Subscribe(_ => {
                SpawnEnemy();
            }).AddTo(this);
    }
    
    private void SpawnEnemy()
    {
        if(playerHealth.isDead)
            return;
        if (spawnPoints.Length > 0)
        {
            var spawnIndex = Random.Range(0, spawnPoints.Length);
            var spawnPoint = spawnPoints[spawnIndex];

            // 확률에 따라 적 선택
            var selectedEnemyPrefab = SelectEnemyPrefab();
            if (selectedEnemyPrefab != null)
            {
                var enemy = GetPooledEnemy(spawnPoint, selectedEnemyPrefab);
                if (enemy != null)
                {
                    enemy.transform.position = spawnPoint.position;
                    enemy.transform.rotation = spawnPoint.rotation;
                    enemy.SetActive(true);
                }
            }
        }
    }

    private GameObject SelectEnemyPrefab()
    {
        if (enemyPrefabs.Count > 0)
        {
            var totalWeight = enemyPrefabs.Sum(prefab => prefab.GetComponent<Enemy>().enemyData.spawnPercent);
            var randomValue = Random.Range(0, totalWeight);
            var weightSum = 0;
            foreach (var prefab in enemyPrefabs)
            {
                weightSum += prefab.GetComponent<Enemy>().enemyData.spawnPercent;
                if (randomValue <= weightSum)
                {
                    return prefab;
                }
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    private GameObject GetPooledEnemy(Transform spawnPoint, GameObject selectedEnemyPrefab)
    {
        var enemy = enemyPool.Find(e => !e.activeInHierarchy && e.CompareTag(selectedEnemyPrefab.tag));
        if (enemy == null)
        {
            enemy = CreateNewEnemy(spawnPoint);
        }
        else
        {
            ResetEnemy(enemy, spawnPoint);
        }
        return enemy;
    }

    private GameObject CreateNewEnemy(Transform spawnPoint)
    {
        if (enemyPrefabs.Count > 0)
        {
            var prefabIndex = Random.Range(0, enemyPrefabs.Count);
            var newEnemy = Instantiate(enemyPrefabs[prefabIndex], spawnPoint.position, spawnPoint.rotation, spawnPoint);
            enemyPool.Add(newEnemy);
            return newEnemy;
        }
        return null;
    }
    
    private void ResetEnemy(GameObject enemy,Transform spawnPoint)
    {
        enemy.transform.SetParent(spawnPoint, false);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        
        var navMeshAgent=enemy.GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        var boxCollider = enemy.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        
        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.health = enemy.GetComponent<Enemy>().enemyData.maxHealth;
    }
    
}
