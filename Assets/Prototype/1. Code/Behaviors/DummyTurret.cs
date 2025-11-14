using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTurret : MonoBehaviour
{
    #region Public Variables 
    [SerializeField] private GameObject _dummyBullet;
    private DummyEnemy _dummy;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _fireTime;
    [SerializeField] private Vector2 _bulletDirection;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dummy = GetComponent<DummyEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    private void Shoot()
    {
        if (!_dummy.IsStunned)
        {
            _fireTime += Time.deltaTime;

            if (_fireTime >= _fireRate)
            {
                var dummyBullet = _dummyBullet.GetComponent<Dummy_Bullet>();
                dummyBullet.SetDirection(_bulletDirection);
                var bullet = Instantiate(_dummyBullet, transform.position, Quaternion.identity);
                _fireTime = 0;
            }
        }
        else
        {
            return;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
