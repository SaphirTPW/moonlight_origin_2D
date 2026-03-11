using UnityEngine;

public class RageArmor : Passive
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
        //Debug.Log("Value Updated");
        Pm.RageArmorOn = true;
    }

    public override void DisablePassive()
    {
        Pm.RageArmorOn = false;
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
