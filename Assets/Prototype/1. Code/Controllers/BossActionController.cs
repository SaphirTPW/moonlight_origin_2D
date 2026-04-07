using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossActionController : MonoBehaviour
{
    [SerializeField] private List<BossActionSO> _actionSOList;
    private List<BossAction> _actions = new List<BossAction>();
    private int _currentActionIndex = 0;

    [SerializeField] private Transform _bossTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private LayerMask _groundLayer;

    private void Awake()
    {
        _bossTransform = transform;
    }

    private void Start()
    {
        foreach (var actionSO in _actionSOList)
        {
            var action = new SlamAttackAction(actionSO as SlamAttackSO, _bossTransform, _playerTransform, _groundLayer);
            action.ActionFinished += OnActionFinished;
            _actions.Add(action);
        }

        if(_actions.Count > 0)
        {
            _actions[0].StartAction();
        }
    }

    private void Update()
    {
        if (_actions.Count == 0) return;
            _actions[_currentActionIndex].UpdateAction();
    }

    private void OnActionFinished()
    {
        _currentActionIndex++;
        if (_currentActionIndex >= _actions.Count)
            _currentActionIndex = 0;
        _actions[_currentActionIndex].StartAction();
    }
}
