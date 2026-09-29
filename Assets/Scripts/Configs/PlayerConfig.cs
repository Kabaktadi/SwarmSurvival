using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfigObject", menuName = "Configs/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    public float Health = 100f;
    public float Speed = 0.2f;
    public float FireCooldown = 0.1f;
    public float ProjectileSpeed = 700f;
}
