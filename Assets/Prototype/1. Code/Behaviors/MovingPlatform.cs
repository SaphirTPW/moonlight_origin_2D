using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private float _speed;

    [SerializeField] private float _waitTime = 1f;
    private float _waitTimer = 0f;
    private bool _isWaiting;

    private Transform _target;
    private Vector3 _lastPosition;

    private Transform _playerOnPlatform;

    private void Start()
    {
        _target = _pointB;
        _lastPosition = transform.position;
    }

    private void Update()
    {
        MovePlatform();
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - _lastPosition;

        if(_playerOnPlatform != null)
        {
            _playerOnPlatform.position += delta;
        }

        _lastPosition = transform.position;
    }

    private void MovePlatform()
    {
        if (_isWaiting)
        {
            _waitTimer += Time.deltaTime;

            if(_waitTimer >= _waitTime)
            {
                _waitTimer = 0f;
                _isWaiting = false;

                _target = (_target == _pointA) ? _pointB : _pointA;
            }

            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            _target.position,
            _speed * Time.deltaTime
            );

        if(Vector3.Distance(transform.position, _target.position) < 0.05f)
        {
            _isWaiting = true;
            _waitTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerOnPlatform = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _playerOnPlatform = null;
        }
    }
}
