using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Unique Skill")]
public class UniqueSkillData : ScriptableObject
{
    #region Public Variables 
    public string uSkillName;
    public string uSkillDesc;
    public USkillEmoType uSkillEmoType;
    public float coolDownTime;

    public enum USkillEmoType
    {
        Neutral,
        Joy,
        Anger,
        Sadness,
        Fear
    }

    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
