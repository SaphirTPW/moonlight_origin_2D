using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUpAnger : Passive
{
    #region Public Variables 
    private float _buildUpDamage;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
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
        //PCom.AttackDamage = PCom.AttackDamage + 5;
    }

    public override void DisablePassive() 
    {
        
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
