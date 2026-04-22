using UnityEngine;

public class CubeDashAction : BossAction
{
    private CubeDashSO _data;

    private Transform _bossTransform;
    private Transform _playerTransform;

    private int _currentDash = 0;
    private float _timer = 0f;

    private Vector2 _dashDirection;

    private CubeDashPhase _currentPhase;

    public enum CubeDashPhase
    {
        Teleport,
        WindUp,
        Dashing,
        Waiting,
        Recovery
    }

    public CubeDashAction(CubeDashSO data, Transform boss, Transform player) : base(data)
    {
        _data = data;
        _bossTransform = boss;
        _playerTransform = player;
    }

    public override void StartAction()
    {
        _currentDash = 0;
        _timer = 0f;
        _currentPhase = CubeDashPhase.Teleport;
    }

    public override void UpdateAction()
    {
        switch (_currentPhase)
        {
            case CubeDashPhase.Teleport:
                DoTeleport();
                break;
            case CubeDashPhase.WindUp:
                _timer += Time.deltaTime;
                if(_timer >= _data.preDashDelay)
                {
                    _timer = 0f;
                    LockDirection();
                    _currentPhase = CubeDashPhase.Dashing;
                }
                break;
            case CubeDashPhase.Dashing:
                DoDash();
                break;
            case CubeDashPhase.Waiting:
                _timer += Time.deltaTime;
                if(_timer >= _data.delayBetweenDash)
                {
                    _timer = 0f;
                    _currentDash++;

                    if (_currentDash < _data.dashCount)
                        _currentPhase = CubeDashPhase.Teleport;
                    else
                        _currentPhase = CubeDashPhase.Recovery;
                }
                break;
            case CubeDashPhase.Recovery:
                _timer += Time.deltaTime;
                if(_timer >= _data.finalRecoveryTime)
                {
                    NotifyFinished();
                }
                break;
        }
    }

    private void DoTeleport()
    {
        //Check in which side CubeMan will appear when he teleports
        float side = Random.value > 0.5f ? 1f : -1f;

        Vector3 targetPos = _playerTransform.position;

        targetPos.x += side * _data.teleportOffsetX;
        targetPos.y += _data.teleportOffsetY;

        _bossTransform.position = targetPos;

        _timer = 0f;
        _currentPhase = CubeDashPhase.WindUp;
    }

    private void LockDirection()
    {
        Vector2 dir = (_playerTransform.position - _bossTransform.position).normalized;
        _dashDirection = dir;
    }

    private void DoDash()
    {
        _bossTransform.position += (Vector3)(_dashDirection * _data.dashSpeed * Time.deltaTime);

        _timer += Time.deltaTime;

        if(_timer >= _data.dashDuration || HitWall())
        {
            _timer = 0f;
            _currentPhase = CubeDashPhase.Waiting;
        }
    }

    private bool HitWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _bossTransform.position,
            _dashDirection,
            0.5f,
            LayerMask.GetMask("Ground", "Wall")
            );

        return hit.collider != null;
    }
}
