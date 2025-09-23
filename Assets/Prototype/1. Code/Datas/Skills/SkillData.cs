using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Skill")]
public class SkillData : ScriptableObject
{
    #region Public Variables 
    public string skillName;
    public string skillDesc;
    public SkillEmoType skillEmoType;
    public float coolDownTime;
    public float skillCost;

    public enum SkillEmoType
    {
        Neutral,
        Joy,
        Anger,
        Sadness,
        Fear
    }
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
