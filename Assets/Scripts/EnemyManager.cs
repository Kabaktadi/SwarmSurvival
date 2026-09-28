using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyFactory[] factories;

    private EnemyFactory _factory;
    private float _spawnTimer = 0f;
    private float _spawnInterval = 1f;
    
    private void SpawnEnemy()
    {
        _factory = factories[Random.Range(0, factories.Length)];
        
        float posX = Random.Range(-10.0f, 10.0f);
        float posZ = Random.Range(-10.0f, 10.0f);
        
        Vector3 position = new Vector3(posX, 0.5f, posZ);

        if (_factory != null)
        {
            _factory.CreateEnemy(position);
        }
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        while (_spawnTimer >= _spawnInterval)
        {
            SpawnEnemy();
            _spawnTimer -= _spawnInterval;
        }
    }
}
