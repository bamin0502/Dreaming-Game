
using System;
using System.Collections.Generic;
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
    
    
    void Start()
    {
        
        Observable.Interval(System.TimeSpan.FromSeconds(spawnInterval))
            .Subscribe(_ => {
                SpawnEnemy();
            }).AddTo(this);
    }
    [Tooltip("테스트용 키 입력 함수 나중에 제거 예정")]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))  // X 키를 누르면 모든 적을 제거
        {
            RemoveAllEnemies();
        }
    }
    void SpawnEnemy()
    {
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
    GameObject SelectEnemyPrefab()
    {
        if (enemyPrefabs.Count > 0)
        {
            var prefabIndex = Random.Range(0, enemyPrefabs.Count);
            return enemyPrefabs[prefabIndex];
        }
        return null;
    }
    GameObject GetPooledEnemy(Transform spawnPoint, GameObject selectedEnemyPrefab)
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                Debug.Log("풀에서 적을 재사용합니다.");
                ResetEnemy(enemy, spawnPoint);
                return enemy;
            }
        }
        Debug.Log("풀에 적이 없어 새로 생성합니다.");
        return CreateNewEnemy(spawnPoint);
    }

    GameObject CreateNewEnemy(Transform spawnPoint)
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
    void RemoveAllEnemies()
    {
        foreach (var enemy in enemyPool)
        {
            if (enemy.activeInHierarchy)
            {
                enemy.SetActive(false);  // 적을 비활성화하여 풀로 반환
            }
        }
        Debug.Log("모든 적을 제거했습니다.");
    }

    private void ResetEnemy(GameObject enemy,Transform spawnPoint)
    {
        enemy.transform.SetParent(spawnPoint, false);
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        
        var navMeshAgent=enemy.GetComponent<NavMeshAgent>();
        //navMeshAgent.isStopped = false;
        navMeshAgent.enabled = true;
        var boxCollider = enemy.GetComponent<BoxCollider>();
        boxCollider.enabled = true;
        
        var enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.health = enemy.GetComponent<Enemy>().enemyData.maxHealth;
    }
    
}
