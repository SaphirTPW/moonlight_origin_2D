using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/BossAction")]
public class BossActionSO : ScriptableObject
{
    public string actionName;
    public float duration;
    public float speed;
    //public float damage;
    //public float upKnockback;
    //public float knockBackForce;
}
