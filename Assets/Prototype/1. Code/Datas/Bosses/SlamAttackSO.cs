using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/BossAction/SlamAttack")]
public class SlamAttackSO : BossActionSO
{
    public float followDuration = 2f;
    public float fallSpeed = 10f;
    public int repetitions = 3;

    public GameObject shockWavePrefab;
}
