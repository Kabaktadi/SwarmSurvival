using UnityEngine;

public class EnemyBoxFactory : EnemyFactory
{
    [SerializeField] private EnemyBox _enemyBox;
    
    public override IEnemy CreateEnemy(Vector3 position)
    {
        if (_enemyBox != null)
        {
            GameObject _enemyBoxInstance = Instantiate(_enemyBox.gameObject, position, Quaternion.identity);
            EnemyBox enemyBox = _enemyBoxInstance.gameObject.GetComponent<EnemyBox>();
        
            enemyBox.Initialize();

            return enemyBox;
        }
       
        return null;
    }
}
