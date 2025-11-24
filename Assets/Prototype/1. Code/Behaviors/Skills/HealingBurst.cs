using UnityEngine;

public class HealingBurst : Skill
{
    private float _chargeHealBurstTime;
    [SerializeField] private float _maxChargeHealBurstTime;
    [SerializeField] private float _healingAmount;
    [SerializeField] private ParticleSystem _gatherFX;
    [SerializeField] private ParticleSystem _busrtFX;
    private bool _isActive = false;

    #region Unity Methods 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (CurrentSkillState == SkillState.Ready)
        {
            HandleCharge();
        }
    }
    #endregion

    #region Public Methods 
    public override void EnableSkill(float pSkillCost)
    {
        if (!_isActive)
        {
            base.EnableSkill(pSkillCost);
            _isActive = true;
        }
    }

    public override void SkillOnCoolDown()
    {
        base.SkillOnCoolDown();
    }

    public override void SetSkillInfo()
    {
        base.SetSkillInfo();
    }
    #endregion

    #region Private Methods 

    private void HandleCharge()
    {
        if (_isActive)
        {
            _chargeHealBurstTime += Time.deltaTime;
            if (_chargeHealBurstTime != _maxChargeHealBurstTime)
            {
                PC.CanMove = false;
                PM.Rb.linearVelocity = Vector2.zero;
                PH.IsHealing = false;
                _gatherFX.Play();
            }

            if (_chargeHealBurstTime >= _maxChargeHealBurstTime)
            {
                _gatherFX.Stop();
                HandleHealBurst(_healingAmount);
                _busrtFX.Play();
                PC.CanMove = true;
                PH.IsHealing = true;
                _chargeHealBurstTime = 0;
                CurrentSkillState = SkillState.CoolDown;
            }
        }
    }

    private void HandleHealBurst(float pAmount)
    {
        pAmount = PH.PlayerMaxHealth * 0.4f;
        
        if (_isActive)
        {
            
            Debug.Log(pAmount);
            PH.PlayerCurrentHealth += pAmount;
            _isActive = false;
        }

        if (PH.PlayerCurrentHealth >= PH.PlayerMaxHealth)
            PH.PlayerCurrentHealth = PH.PlayerMaxHealth;
    }

    #endregion
}
