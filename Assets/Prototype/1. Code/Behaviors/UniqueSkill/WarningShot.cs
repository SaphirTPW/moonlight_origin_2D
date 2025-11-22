using UnityEngine;

public class WarningShot : UniqueSkill
{
    [SerializeField] private GameObject _warningShotObj;
    [SerializeField] private Transform _warningShotPoint;


    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void EnableUSkill()
    {
        base.EnableUSkill();
        if (CurrentUSkillState == USkillState.Ready)
        {
            HandleWarningShotSkill();
        }
    }

    public override void USkillOnCoolDown()
    {
        base.USkillOnCoolDown();
    }

    private void HandleWarningShotSkill()
    {
        if (CurrentUSkillState == USkillState.Ready)
        {
            var _warningBullet = _warningShotObj.GetComponent<Bullet>();
            float dir = Mathf.Sign(transform.localScale.x);
            _warningBullet.SetDirection(new Vector2(dir, 0));

            var bullet = Instantiate(_warningShotObj, _warningShotPoint.position, Quaternion.identity);
            bullet.transform.localScale = Vector3.one;
            CurrentUSkillState = USkillState.CoolDown;
        }
    }
}
