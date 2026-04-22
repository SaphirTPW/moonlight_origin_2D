using System;
using UnityEngine;

public abstract class BossAction
{
    public event Action ActionFinished;
    protected BossActionSO _actionData;

    public BossAction(BossActionSO pActionData)
    {
        _actionData = pActionData;
    }

    public abstract void StartAction();
    public abstract void UpdateAction();
    protected void NotifyFinished()
    {
        ActionFinished?.Invoke();
    }
}
