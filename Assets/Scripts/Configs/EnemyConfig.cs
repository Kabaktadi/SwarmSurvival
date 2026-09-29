using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfigObject", menuName = "Configs/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public float Speed = 1.5f;
    public float Damage = 10f;
}
