using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MoreMountains.TopDownEngine;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameProgressionService : MonoBehaviour
{

    [Serializable]
    public class EnemyVariant
    {
        public GameObject enemyPrefab;
        public float spawnTimeout;
        public float spawnHealthMultiplier = 1.0f;
        public bool ignore = true;
    }

    public int maxEnemies = 10;
    public List<EnemyVariant> enemyVariants;
    public List<Transform> spawnPoints;

    private float _currentTimeout = 0.0f;
    private List<GameObject> _spawnedEnemies;

    public void AddEnemyVariant(EnemyVariant enemyVariant) => enemyVariants.Add(enemyVariant);

    public void AddSpawnPoint(Transform spawnPoint) => spawnPoints.Add(spawnPoint);

    public void ToggleEnemyVariant(int id, bool targetState) => enemyVariants[id].ignore = targetState;

    public void UpdateAllEnemyVariants(Action<EnemyVariant> action) => enemyVariants.ForEach(action);
    
    public void UpdateActiveEnemyVariants(Action<EnemyVariant> action) => enemyVariants.ForEach(x =>
    {
        if (!x.ignore)
        {
            action(x);
        }
    });

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
        var availableEnemyVariants = enemyVariants.Where(x => !x.ignore).ToList();
        var v = enemyVariants[Random.Range(0, availableEnemyVariants.Count)];
        // Get a random spawn point
        var s = spawnPoints[Random.Range(0, spawnPoints.Count)];
        // Spawn the entity
        var enemy = Instantiate(v.enemyPrefab, s.transform.position, s.rotation);
        _spawnedEnemies.Add(enemy);
        // Add timeout
        _currentTimeout += v.spawnTimeout;

        var health = enemy.GetComponent<Health>();
        health.MaximumHealth = (int)(health.MaximumHealth * v.spawnHealthMultiplier);
        health.CurrentHealth = (int)(health.CurrentHealth * v.spawnHealthMultiplier);
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
