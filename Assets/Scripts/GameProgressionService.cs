using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressionService : MonoBehaviour
{

    [System.Serializable]
    public class EnemyVariant
    {
        public GameObject enemyPrefab;
        public float spawnTimeout;
    }
    
    public List<EnemyVariant> enemyVariants;
    public List<Transform> spawnPoints;

    private float _currentTimeout = 0.0f;

    public void AddEnemyVariant(EnemyVariant enemyVariant) => enemyVariants.Add(enemyVariant);

    public void AddSpawnPoint(Transform spawnPoint) => spawnPoints.Add(spawnPoint);

    private void FixedUpdate()
    {
        // Wait for timeout
        if (_currentTimeout > 0)
        {
            _currentTimeout = Mathf.Max(0.0f, _currentTimeout - Time.fixedDeltaTime);
            return;
        }
        
        // Get a random variant
        var v = enemyVariants[Random.Range(0, enemyVariants.Count)];
        // Get a random spawn point
        var s = spawnPoints[Random.Range(0, spawnPoints.Count)];
        // Spawn the entity
        var enemy = Instantiate(v.enemyPrefab, s.transform.position, s.rotation);
        // Add timeout
        _currentTimeout += v.spawnTimeout;
    }
}
