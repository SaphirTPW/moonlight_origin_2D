using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer _spriteRenderer;
    private Material _material;

    private Coroutine _damageFlashCoroutine;
    #endregion

    #region Unity Methods 

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Public Methods 
    public void CallDamageFlash()
    {
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    #endregion

    #region Private Methods 
    private void Init()
    {
        _material = _spriteRenderer.material;
    }

    private void SetFlashColor()
    {
        //for (int i = 0; i < _materials.Length; i++)
        //{
        //    _materials[i].SetColor("_FlashColor", _flashColor);
        //}
        _material.SetColor("_FlashColor", _flashColor);
    }

    private void SetFloatAmount(float pAmount)
    {
        //for (int i = 0; i < _materials.Length; i++)
        //{
        //    _materials[i].SetFloat("_FlashAmount", pAmount);
        //}
        _material.SetFloat("_FlashAmount", pAmount);
    }
    #endregion

    #region Coroutines
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float elaspedTime = 0f;

        while(elaspedTime < _flashTime)
        {
            elaspedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elaspedTime/_flashTime) );
            SetFloatAmount(currentFlashAmount);
            yield return null;
        }
    }
    #endregion
}
