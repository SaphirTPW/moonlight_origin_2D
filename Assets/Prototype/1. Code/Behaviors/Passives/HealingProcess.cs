using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingProcess : Passive
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
    private void Start()
    {

    }
    public override void Update()
    {
        base.Update();
    }
    #endregion

    #region Public Methods 
    public override void UpdatePassiveState(PassiveState pPassiveState)
    {
        base.UpdatePassiveState(pPassiveState);
    }
    public override void HandlePassiveOn()
    {
        base.HandlePassiveOn();
    }

    public override void EnablePassive()
    {
        PH.IsHealing = true;
    }

    public override void DisablePassive()
    {
        PH.IsHealing = false;
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
