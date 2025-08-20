using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Emotion")]
public class EmotionData : ScriptableObject
{
    #region Public Variables 
    public string emotionName;
    public EmotionType emotionType;

    public float powerModifier;
    public float defenseModifier;
    public float speedModifier;
    public float moveSmoothing;

    public float coPowerModifier;
    public float coDefenseModifier;
    public float coSpeedModifier;
    public float coMoveSmoothing;

    public float buildUpRate;
    public float coolDownRate;
    public float crashCoolDownRate;

    public enum EmotionType
    {
        Neutral,
        Joy,
        Anger,
        Sadness,
        Fear
    }
    #endregion

}
