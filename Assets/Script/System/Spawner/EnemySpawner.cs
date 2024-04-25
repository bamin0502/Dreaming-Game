using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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

    void SpawnEnemy()
    {
        if (spawnPoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[spawnIndex];
            GameObject enemy = GetPooledEnemy(spawnPoint);
            if (enemy != null)
            {
                enemy.transform.position = spawnPoint.position;
                enemy.transform.rotation = spawnPoint.rotation;
                enemy.SetActive(true);
            }
        }
    }

    GameObject GetPooledEnemy(Transform spawnPoint)
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
            {
                enemy.transform.SetParent(spawnPoint);
                enemy.transform.localPosition = Vector3.zero; 
                enemy.transform.localRotation = Quaternion.identity; 
                return enemy;
            }
        }
        return CreateNewEnemy(spawnPoint);
    }

    GameObject CreateNewEnemy(Transform spawnPoint)
    {
        if (enemyPrefabs.Count > 0)
        {
            int prefabIndex = Random.Range(0, enemyPrefabs.Count);
            GameObject newEnemy = Instantiate(enemyPrefabs[prefabIndex], spawnPoint.position, spawnPoint.rotation, spawnPoint);
            enemyPool.Add(newEnemy);
            return newEnemy;
        }
        return null;
    }
}
