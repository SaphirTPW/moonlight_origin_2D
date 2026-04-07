using UnityEngine;

public class SlamAttackAction : BossAction
{
    private SlamAttackSO _actionData;
    private int _currentReps = 0;
    private float _timer = 0f;
    private float _riseSpeed = 5f;

    private Transform _bossTransform;
    private Transform _target;

    private LayerMask _groundLayer;

    private float _initialY;

    private SlamAttackPhase _currentPhase = SlamAttackPhase.Follow;

    public SlamAttackAction(SlamAttackSO data, Transform bossTransform, Transform target, LayerMask groundLayer) : base(data)
    {
        _actionData = data;
        _bossTransform = bossTransform;
        _target = target;
        _groundLayer = groundLayer;
    }

    public enum SlamAttackPhase
    {
        Follow,
        Falling,
        Rising
    }

    public override void StartAction()
    {
        _timer = 0;
        _currentReps = 0;
        _currentPhase = SlamAttackPhase.Follow;
        _initialY = _bossTransform.position.y;
    }

    public override void UpdateAction()
    {
        switch (_currentPhase)
        {
            case SlamAttackPhase.Follow:
                float followSpeed = 5f;
                float maxOffset = 2f;

                // Calculer une position cible avec offset aléatoire
                float targetX = _target.position.x + Random.Range(-maxOffset, maxOffset);

                float newX = Mathf.Lerp(_bossTransform.position.x, targetX, followSpeed * Time.deltaTime);
                _bossTransform.position = new Vector3(newX, _bossTransform.position.y, _bossTransform.position.z);
                
                _timer += Time.deltaTime;
                if(_timer >= _actionData.followDuration)
                {
                    _timer = 0f;
                    _currentPhase = SlamAttackPhase.Falling;
                }
                break;
            case SlamAttackPhase.Falling:
                // Déplacement
                _bossTransform.position += Vector3.down * _actionData.fallSpeed * Time.deltaTime;

                // Raycast 2D depuis le bas du boss
                Vector2 rayOrigin = new Vector2(_bossTransform.position.x, _bossTransform.position.y - (_bossTransform.localScale.y / 2f));
                float rayDistance = _actionData.fallSpeed * Time.deltaTime + 0.2f;

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, _groundLayer);
                if (hit.collider != null)
                {
                    Debug.Log("Impacted at y=" + hit.point.y);
                    OnImpact();
                }

                Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red);
                break;
            case SlamAttackPhase.Rising:
                Debug.Log("Rising");
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
                        NotifyFinised();
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

        _currentReps++;
        _currentPhase = SlamAttackPhase.Rising;
    }

}
