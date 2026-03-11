using UnityEngine;

public class GriefFacade : Passive
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
        PH.IsGriefFaceOn = true;
    }

    public override void DisablePassive()
    {
        PH.IsGriefFaceOn = false;
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
