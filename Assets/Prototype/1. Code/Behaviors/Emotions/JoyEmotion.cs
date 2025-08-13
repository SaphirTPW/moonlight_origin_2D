using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Emotion;

public class JoyEmotion : Emotion
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
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
