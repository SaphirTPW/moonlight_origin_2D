using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/BossAction/CubeOrb")]
public class CubeOrbSO : BossActionSO
{
    public float spawnDelay = 0.5f;
    public int orbCount = 3;

    public float followDuration = 2f;
    public float followSpeed = 5f;

    public float projectileSpeed = 8f;

    public GameObject orbPrefab;
}
