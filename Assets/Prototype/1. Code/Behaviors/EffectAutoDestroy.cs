using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAutoDestroy : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    private Animator _fxAnimator;
    private float _animationTime;
    #endregion

    #region Unity Methods 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fxAnimator = GetComponent<Animator>();

        _animationTime = _fxAnimator.GetCurrentAnimatorStateInfo(0).length;

        Destroy(gameObject, _animationTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods 
    #endregion

    #region Private Methods 
    #endregion

    #region Coroutines
    #endregion
}
