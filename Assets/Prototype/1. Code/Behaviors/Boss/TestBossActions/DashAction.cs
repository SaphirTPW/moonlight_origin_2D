using System.Threading;
using UnityEngine;

public class DashAction : BossAction
{
    private float _timer = 0f;

    public DashAction(BossActionSO data) : base(data)
    {

    }

    public override void StartAction()
    {
        Debug.Log($"DashAction Start ! Duration: {_actionData.duration}, Speed: {_actionData.speed}");
    }

    public override void UpdateAction()
    {
        _timer += Time.deltaTime;
        Debug.Log("Dash en cours...");
    }
}
