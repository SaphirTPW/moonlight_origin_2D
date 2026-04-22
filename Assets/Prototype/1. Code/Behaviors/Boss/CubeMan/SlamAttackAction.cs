using UnityEngine;

public class SlamAttackAction : BossAction
{
    private SlamAttackSO _actionData;
    private int _currentReps = 0;
    private float _timer = 0f;
    private float _riseSpeed = 5f;
    private float _waitDuration;

    private Transform _bossTransform;
    private Transform _target;
    private GameObject _impactZone;

    private LayerMask _groundLayer;

    private float _initialY;

    private SlamAttackPhase _currentPhase = SlamAttackPhase.Follow;

    public SlamAttackAction(SlamAttackSO data, Transform bossTransform, Transform target, LayerMask groundLayer, GameObject impactZone) : base(data)
    {
        _actionData = data;
        _bossTransform = bossTransform;
        _target = target;
        _groundLayer = groundLayer;
        _impactZone = impactZone;
    }

    public enum SlamAttackPhase
    {
        Follow,
        Falling,
        Wating,
        Rising
    }

    public override void StartAction()
    {
        _timer = 0;
        _currentReps = 0;
        //_initialY = _bossTransform.position.y;
        _initialY = 8f;

        if (_bossTransform.position.y < _initialY)
        {
            _currentPhase = SlamAttackPhase.Rising;
        }
        else
            _currentPhase = SlamAttackPhase.Follow;

        _impactZone.SetActive(false);
    }

    public override void UpdateAction()
    {
        switch (_currentPhase)
        {
            case SlamAttackPhase.Follow:
                //CubeMan Smooth Follow 
                float followSpeed = 5f;
                float maxOffset = 2f;

                float targetX = _target.position.x + Random.Range(-maxOffset, maxOffset);

                float newX = Mathf.Lerp(_bossTransform.position.x, targetX, followSpeed * Time.deltaTime);
                _bossTransform.position = new Vector3(newX, _bossTransform.position.y, _bossTransform.position.z);

                _timer += Time.deltaTime;
                if (_timer >= _actionData.followDuration)
                {
                    _timer = 0f;
                    _currentPhase = SlamAttackPhase.Falling;
                }
                break;
            case SlamAttackPhase.Falling:
                // Down Movement
                _bossTransform.position += Vector3.down * _actionData.fallSpeed * Time.deltaTime;
                _impactZone.SetActive(true);

                Vector2 rayOrigin = new Vector2(_bossTransform.position.x, _bossTransform.position.y - (_bossTransform.localScale.y / 2f));
                float rayDistance = _actionData.fallSpeed * Time.deltaTime + 0.2f;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, _groundLayer);
                
                if (hit.collider != null)
                {
                    //Debug.Log("Impacted at y=" + hit.point.y);
                    OnImpact();
                }

                Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red);
                break;
            case SlamAttackPhase.Wating:
                _timer += Time.deltaTime;

                if (_timer >= _waitDuration)
                {
                    _timer = 0f;
                    _currentPhase = SlamAttackPhase.Rising;
                }
                break;
            case SlamAttackPhase.Rising:
                //Debug.Log("Rising");
                Vector3 pos = _bossTransform.position;
                pos.y = Mathf.Lerp(pos.y, _initialY, _riseSpeed * Time.deltaTime);
                _bossTransform.position = pos;

                if(Mathf.Abs(pos.y - _initialY) < 0.05f)
                {
                    pos.y = _initialY;
                    _bossTransform.position = pos;
                    _timer = 0;

                    if (_currentReps < _actionData.repetitions)
                        _currentPhase = SlamAttackPhase.Follow;
                    else
                        NotifyFinished();
                }
                break;
            default:
                break;
        }
    }

    private void OnImpact()
    {
        if(_actionData.shockWavePrefab != null)
        {
            GameObject.Instantiate(_actionData.shockWavePrefab, _bossTransform.position, Quaternion.identity);
        }

        _impactZone.SetActive(false);
        _currentReps++;

        if (_currentReps < _actionData.repetitions)
            _waitDuration = 1f;
        else
            _waitDuration = 3f;

        _timer = 0f;
        _currentPhase = SlamAttackPhase.Wating;
    }

}
