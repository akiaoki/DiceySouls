using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameProgressionService : MonoBehaviour
{

    [System.Serializable]
    public class EnemyVariant
    {
        public GameObject enemyPrefab;
        public float spawnTimeout;
    }

    public int maxEnemies = 10;
    public List<EnemyVariant> enemyVariants;
    public List<Transform> spawnPoints;

    private float _currentTimeout = 0.0f;
    private List<GameObject> _spawnedEnemies;

    public void AddEnemyVariant(EnemyVariant enemyVariant) => enemyVariants.Add(enemyVariant);

    public void AddSpawnPoint(Transform spawnPoint) => spawnPoints.Add(spawnPoint);

    private void Awake()
    {
        _spawnedEnemies = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        // Wait for timeout
        if (_currentTimeout > 0)
        {
            _currentTimeout = Mathf.Max(0.0f, _currentTimeout - Time.fixedDeltaTime);
            return;
        }

        // Do not spawn enemies if limit reached
        if (GetActiveEnemies() >= maxEnemies)
        {
            return;
        }

        // Get a random variant
        var v = enemyVariants[Random.Range(0, enemyVariants.Count)];
        // Get a random spawn point
        var s = spawnPoints[Random.Range(0, spawnPoints.Count)];
        // Spawn the entity
        var enemy = Instantiate(v.enemyPrefab, s.transform.position, s.rotation);
        _spawnedEnemies.Add(enemy);
        // Add timeout
        _currentTimeout += v.spawnTimeout;
    }

    private int GetActiveEnemies()
    {
        var result = 0;
        
        for (var i = 0; i < _spawnedEnemies.Count; i++)
        {
            if (_spawnedEnemies[i] == null || !_spawnedEnemies[i].activeSelf)
            {
                _spawnedEnemies.RemoveAt(i--);
                continue;
            }

            result++;
        }

        return result;
    }
}
