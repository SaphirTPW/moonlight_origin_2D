using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    #region Public Variables 
    #endregion

    #region Private Variables 
    [SerializeField][Range(2f, 2.75f)] private float _damageReduction;
    [SerializeField] private GameObject _player;
   
    #endregion

    #region Unity Methods 
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
    #endregion

    #region Private Methods 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyAttack")
        {
            _player.GetComponent<PlayerHealth>().PlayerTakeDamage(collision.GetComponent<Dummy_Bullet>().BulletDamage / _damageReduction);
            Destroy(collision.gameObject);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}
