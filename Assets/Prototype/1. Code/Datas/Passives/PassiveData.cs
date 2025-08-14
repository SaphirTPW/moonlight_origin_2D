using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Passive")]
public class PassiveData : ScriptableObject
{
    #region Public Variables
    public string passiveName;
    public string passiveDesc;
    public PassiveType passiveType;
    public PassiveEmoType passiveEmoType;

    public enum PassiveType 
    {
        Auto,
        Conditional 
    }

    public enum PassiveEmoType
    {
        Neutral,
        Joy,
        Anger,
        Sadness, 
        Fear
    }
    #endregion
}
