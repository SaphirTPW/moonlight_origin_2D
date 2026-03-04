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
    #endregion

    #region Public Methods 

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
