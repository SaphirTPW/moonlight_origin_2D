using UnityEngine;
using static Skill;

public class UniqueSkill : MonoBehaviour
{
    #region Public Methods 
    public PlayerController PC { get => _pC; set => _pC = value; }
    public EmotionController EC { get => _eC; set => _eC = value; }
    public PlayerCombat PCom { get => _pCom; set => _pCom = value; }
    public PlayerMovement PM { get => _pM; set => _pM = value; }
    public PlayerHealth PH { get => _pH; set => _pH = value; }
    public USkillState CurrentUSkillState { get => _currentUSkillState; set => _currentUSkillState = value; }
    #endregion

    #region Private Variables 
    [SerializeField] private UniqueSkillData _uSkillData;
    [SerializeField] private Emotion _emotion;
    private PlayerHealth _pH;
    private PlayerMovement _pM;
    private PlayerCombat _pCom;
    private EmotionController _eC;
    private PlayerController _pC;

    [SerializeField] private USkillState _currentUSkillState;

    [SerializeField] private float _startCoolDownTime;
    [SerializeField] private float _coolDownTime;
    private string _uSkillName;

    #endregion

    #region Unity Methods
    private void Awake()
    {
        _pH = GetComponent<PlayerHealth>();
        _pM = GetComponent<PlayerMovement>();
        _pCom = GetComponent<PlayerCombat>();
        _eC = GetComponent<EmotionController>();
        _pC = GetComponent<PlayerController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        SetSkillInfo();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        USkillOnCoolDown();
    }
    #endregion

    #region Public Methods 

    public enum USkillState
    {
        Ready,
        CoolDown
    }

    public virtual void SetSkillInfo()
    {
        _startCoolDownTime = _uSkillData.coolDownTime;
        _coolDownTime = _startCoolDownTime;
        _uSkillName = _uSkillData.uSkillName;

    }
    public virtual void EnableUSkill()
    {
        if((int)_uSkillData.uSkillEmoType == (int)_eC.CurrentActiveEmotion)
        {
            if(_currentUSkillState == USkillState.Ready)
            {
                _eC.EmoControllerState = EmotionController.EmotionControllerState.NotReady;
            }
        }
    }

    public virtual void USkillOnCoolDown()
    {
        if (_currentUSkillState == USkillState.CoolDown)
        {
            _coolDownTime -= Time.deltaTime;
            _eC.EmoControllerState = EmotionController.EmotionControllerState.Ready;

            if (_coolDownTime <= 0)
            {
                _currentUSkillState = USkillState.Ready;
                _coolDownTime = _startCoolDownTime;
            }
        }
    }
    #endregion
}
