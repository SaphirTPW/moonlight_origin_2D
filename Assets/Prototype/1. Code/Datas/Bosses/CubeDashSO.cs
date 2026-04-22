using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/BossAction/CubeDash")]
public class CubeDashSO : BossActionSO
{

    public int dashCount = 3;

    public float dashSpeed = 15f;
    public float dashDuration = 1f;

    public float preDashDelay = 0.2f; // wind-up
    public float delayBetweenDash = 1f;
    public float finalRecoveryTime = 2f;

    public float teleportOffsetX = 2f;
    public float teleportOffsetY = 2f;
}
