using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/BossAction/DashAction")]
public class DashActionSO : BossActionSO
{
    public float dashSpeed = 10f;
    public float dashDuration = 1f;
}
