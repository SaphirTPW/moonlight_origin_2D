using UnityEngine;
using static Skill;

public class Slide : UniqueSkill
{
    #region Private Variables
    [SerializeField] private float _dashForce;
    [SerializeField] private float _maxDashTime;
    [SerializeField] private float _dashTime;
    private bool _isActive = false;
    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        HandleSlideCooldown();
    }

    public override void EnableUSkill()
    {
        base.EnableUSkill();
        if(CurrentUSkillState == USkillState.Ready)
        {
            HandleSlideUSkill();
        }
    }

    public override void USkillOnCoolDown()
    {
        base.USkillOnCoolDown();
    }

    private void HandleSlideUSkill()
    {
        if (PM.PlayerGrounded)
        {
            PC.CanJump = false;
            PM.Rb.linearVelocity = new Vector2(PM.Rb.linearVelocity.x * _dashForce, PM.Rb.linearVelocity.y);
            _isActive = true;
        }
    }

    private void HandleSlideCooldown()
    {
        if (_isActive)
        {
            _dashTime += Time.deltaTime * 3f;
            //_savedVelocity = PM.Rb.linearVelocity;
        }

        if (_dashTime >= _maxDashTime)
        {
            _isActive = false;
            PC.CanJump = true;
            CurrentUSkillState = USkillState.CoolDown;
            //PM.Rb.linearVelocity = _savedVelocity;
            _dashTime = 0;
        }

    }
}
