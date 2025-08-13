using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralEmotion : Emotion
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }
    #endregion

    #region Public Methods 
    public override void SetEmotionStat()
    {
        base.SetEmotionStat();
    }

    public override void UpdateEmotionState(EmotionState pEmotionState)
    {
        base.UpdateEmotionState(pEmotionState);
    }

    public override void HandleAwakeEmotion()
    {
        base.HandleAwakeEmotion();
    }

    public override void HandleSleepEmotion()
    {
        base.HandleSleepEmotion();
    }

    public virtual void HandleCrashOut()
    {
        base.HandleCrashOut();
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
