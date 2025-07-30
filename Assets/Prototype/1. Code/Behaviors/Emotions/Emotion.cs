using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emotion : MonoBehaviour
{
    #region Public Variables 
    public EmotionState EmoState { get => _emotionState; set => _emotionState = value; }
    public float CurrentEmotionEnergy { get => _currentEmotionEnergy; set => _currentEmotionEnergy = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private EmotionData _emotionData;
    private PlayerMovement _pm;
    private PlayerCombat _pmCom;
    private EmotionController _ec;

    [SerializeField] private EmotionState _emotionState;
    [SerializeField] private float _maxEmotionEnergy = 100f;
    [SerializeField] private float _currentEmotionEnergy;
    [SerializeField] private float _buildUpRate;
    [SerializeField] private float _coolDownRate;
    [SerializeField] private string _emotionName;
    private float crashOutCooldownRate;

    private float _defenseModifier;
    private float _speedModifier;
    private float _attackModifier;

    private bool _canAwake;

    #endregion

    #region Unity Methods 

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods 
    public enum EmotionState
    {
        Awake,
        Sleep,
        CrashOut
    }
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
