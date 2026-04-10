using UnityEngine;
using System;
using Cinemachine;
using System.Runtime.CompilerServices;

public class OrbProjectile : MonoBehaviour
{
    public event Action OnOrbFinished;

    private Transform _target;

    private float _followDuration;
    private float _followSpeed;
    private float _projectileSpeed;

    private float _timer;
    private bool _isLocked;

    private Vector2 _lockedDirection;

    [SerializeField] private float _damage;
    [SerializeField] private float _knockBackForce;
    [SerializeField] private float _knockBackUp;

    [SerializeField] private float _autoDestroyTime;

    public void Launch(Transform pTarget, float pFollowDuration, float pFollowSpeed, float pProjectileSpeed)
    {
        _target = pTarget;
        _followDuration = pFollowDuration;
        _followSpeed = pFollowSpeed;
        _projectileSpeed = pProjectileSpeed;

        _timer = 0f;
        _isLocked = false;
    }

    private void Update()
    {
        if(_target == null)
        {
            Finish();
            return;
        }

        if (!_isLocked)
        {
            FollowTarget();

            _timer += Time.deltaTime;
            if(_timer >= _followDuration)
            {
                LockDirection();
            }
        }
        else
        {
            MoveForward();
        }

        AutoDestroy();
    }

    private void FollowTarget()
    {
        Vector3 pos = transform.position;
        pos = Vector3.Lerp(pos, _target.position, _followSpeed * Time.deltaTime);
        transform.position = pos;
    }

    private void LockDirection()
    {
        _isLocked = true;

        _lockedDirection = (_target.position - transform.position).normalized;
    }

    private void MoveForward()
    {
        transform.position += (Vector3)_lockedDirection * _projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var playerHealth = collision.GetComponent<PlayerHealth>();
            var playerMovement = collision.GetComponent<PlayerMovement>();

            if (collision.GetComponent<PlayerMovement>().RageArmorOn)
            {
                playerHealth.PlayerTakeDamage(_damage);
                Finish();
            }
            else
            {
                playerHealth.PlayerTakeDamage(_damage);
                playerMovement.PlayerKnockback(transform, _knockBackForce, _knockBackUp);
                Finish();
            }
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Finish();
        }
    }

    private void Finish()
    {
        OnOrbFinished?.Invoke();
        Destroy(gameObject);
    }

    private void AutoDestroy()
    {
        _autoDestroyTime -= Time.deltaTime;

        if (_autoDestroyTime <= 0)
        {
            Destroy(gameObject);
            Finish();
        }

    }
}
