using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Emotion")]
public class EmotionData : ScriptableObject
{
    #region Public Variables 
    public string emotionName;

    public float powerModifier;
    public float defenseModifier;
    public float speedModifier;

    public float buildUpRate;
    public float coolDownRate;
    public float crashCoolDownRate;
    #endregion

}
