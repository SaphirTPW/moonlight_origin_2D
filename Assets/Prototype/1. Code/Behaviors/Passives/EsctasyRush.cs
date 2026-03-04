using UnityEngine;
using UnityEngine.Rendering;

public class EsctasyRush : Passive
{
    #region Public Variables 
    private JoyEmotion _joyEmotion;
    [SerializeField] private float _sprintTime;
    [SerializeField] private float _maxSprintTime;
    private float _sprintSpeed;
    private float _defaultSpeed;

    private float _newAnimSpeed;
    private float _defaultAnimSpeed;

    [SerializeField] private bool _canEsctaDash = false;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 

    protected override void OnEnable()
    {
        base.OnEnable();
        Emotion.OnEmotionStateChanged += TurnOffEsctaDash;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Emotion.OnEmotionStateChanged -= TurnOffEsctaDash;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _joyEmotion = GetComponent<JoyEmotion>();
        _sprintSpeed = Pm.PlayerSpeed * 2;
        _defaultSpeed = Pm.PlayerSpeed;

        _defaultAnimSpeed = 1f;
        _newAnimSpeed = 2f;
    }

    private void Update()
    {
        LoadEsctaDash();
    }
    #endregion

    #region Public Methods 

    public override void HandlePassiveOff()
    {
        UpdatePassiveState(PassiveState.Off);
    }

    public override void EnablePassive()
    {

        Pm.PlayerSpeed = _sprintSpeed * 2;
        PC.PlayerAnim.speed = _newAnimSpeed;
    }

    public override void DisablePassive()
    {
        Pm.PlayerSpeed = _defaultSpeed;
        PC.PlayerAnim.speed = _defaultAnimSpeed;
    }

    public override void CheckCondition()
    {
        if(_joyEmotion.EmoState == Emotion.EmotionState.CrashOut)
        {
            _canEsctaDash = true;
        }
    }

    public void LoadEsctaDash()
    {
        if (_canEsctaDash)
        {
            if (PC.IsMoving && Pm.PlayerGrounded)
            {
                _sprintTime += Time.deltaTime;
                if (_sprintTime >= _maxSprintTime)
                {
                    UpdatePassiveState(PassiveState.On);
                }
            }
            else if (!PC.IsMoving)
            {
                _sprintTime = 0;
                HandlePassiveOff();
            }
        }
    }

    public void TurnOffEsctaDash(Emotion.EmotionState state)
    {
        if (_joyEmotion.EmoState != Emotion.EmotionState.CrashOut)
            _canEsctaDash = false;
    }

    #endregion
}
