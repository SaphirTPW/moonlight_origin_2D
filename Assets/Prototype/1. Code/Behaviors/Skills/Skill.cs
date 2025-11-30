using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    #region Public Variables 
    public PlayerHealth PH { get => _pH; set => _pH = value; }
    public PlayerMovement PM { get => _pM; set => _pM = value; }
    public PlayerCombat PCom { get => _pCom; set => _pCom = value; }
    public EmotionController EC { get => _eC; set => _eC = value; }
    public PlayerController PC { get => _pC; set => _pC = value; }
    public SkillState CurrentSkillState { get => _currentSkillState; set => _currentSkillState = value; }
    public float SkillCost { get => _skillCost; set => _skillCost = value; }
    public float CoolDownTime { get => _coolDownTime; set => _coolDownTime = value; }
    public string SkillName { get => _skillName; set => _skillName = value; }
    public WarmingUp WarmUp { get => _warmUp; set => _warmUp = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private SkillData _skillData;
    [SerializeField] private Emotion _emotion;

    private PlayerHealth _pH;
    private PlayerMovement _pM;
    private PlayerCombat _pCom;
    private EmotionController _eC;
    private PlayerController _pC;
    private WarmingUp _warmUp;

    [SerializeField] private SkillState _currentSkillState;

    [SerializeField] private float _startCoolDownTime;
    [SerializeField] private float _coolDownTime;
    private string _skillName;

    [SerializeField] private float _skillCost;

    #endregion

    #region Unity Methods 
    private void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pM = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _eC = GetComponent<EmotionController>();
        _pC = GetComponent<PlayerController>();
        _warmUp = GetComponent<WarmingUp>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        SetSkillInfo();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        SkillOnCoolDown();
    }
    #endregion

    #region Public Methods 
    public enum SkillState
    {
        Ready,
        CoolDown
    }

    public virtual void SetSkillInfo()
    {
        _skillCost = _skillData.skillCost;
        _startCoolDownTime = _skillData.coolDownTime;
        _coolDownTime = _startCoolDownTime;
        _skillName = _skillData.skillName;
    }

    public virtual void EnableSkill (float pSkillCost)
    {
        if ((int)_skillData.skillEmoType == (int)_eC.CurrentActiveEmotion)
        {
            if (_currentSkillState == SkillState.Ready)
            {
                _emotion.CurrentEmotionEnergy += pSkillCost;
                _eC.EmoControllerState = EmotionController.EmotionControllerState.NotReady;
            }
        }
    }

    public virtual void SkillOnCoolDown()
    {
        if(_currentSkillState == SkillState.CoolDown)
        {
            _coolDownTime -= Time.deltaTime;
            _eC.EmoControllerState = EmotionController.EmotionControllerState.Ready;
            
            if (_coolDownTime <= 0)
            {
                _currentSkillState = SkillState.Ready;
                _coolDownTime = _startCoolDownTime;
            }
        }
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
