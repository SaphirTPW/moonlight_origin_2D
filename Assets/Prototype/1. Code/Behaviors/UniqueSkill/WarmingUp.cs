using Unity.Jobs;
using UnityEngine;

public class WarmingUp : UniqueSkill
{
    public bool IsWarmnedUp { get => _isWarmnedUp; set => _isWarmnedUp = value; }

    private float _newDamageMultiplier = 0.25f;
    [SerializeField] private bool _isWarmnedUp = false;
    [SerializeField] private ParticleSystem _warmUpFX;

    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //HandleWarmingUp();
    }

    public override void EnableUSkill()
    {
        base.EnableUSkill();
        if (CurrentUSkillState == USkillState.Ready)
        {
            InitWarmingUp();
        }
    }

    public override void USkillOnCoolDown()
    {
        base.USkillOnCoolDown();
    }

    private void InitWarmingUp()
    {
        if (!_isWarmnedUp)
        {
            _isWarmnedUp = true;
            PCom.DamageMultiplier = PCom.DamageMultiplier + _newDamageMultiplier;
            _warmUpFX.Play();
            EC.EmoControllerState = EmotionController.EmotionControllerState.Ready;
        }
    }

    private void HandleWarmingUp()
    {
        //if (!_isWarmnedUp)
        //{
        //    PCom.DamageMultiplier = 1f;
        //}
    }
}
