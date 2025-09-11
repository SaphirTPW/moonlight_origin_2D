using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTurret : MonoBehaviour
{
    #region Public Variables 
    [SerializeField] private GameObject _dummyBullet;
    [SerializeField] private float _fireRate;
    [SerializeField] private float _fireTime;
    #endregion

    #region Private Variables 
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        _fireTime += Time.deltaTime;

        if(_fireTime >= _fireRate)
        {
            Instantiate(_dummyBullet, transform.position, Quaternion.identity);
            _fireTime = 0;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
