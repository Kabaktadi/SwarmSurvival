using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileConfigObject", menuName = "Configs/ProjectileConfig")]
public class ProjectileConfig : ScriptableObject
{
    public float TimeoutDelay = 3f;
}
