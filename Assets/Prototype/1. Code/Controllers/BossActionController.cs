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
    [SerializeField] private GameObject _impactZone;

    private void Awake()
    {
        _bossTransform = transform;
    }

    private void Start()
    {
        foreach (var actionSO in _actionSOList)
        {
            BossAction action = CreateAction(actionSO);

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

    private BossAction CreateAction(BossActionSO pActionSO)
    {
        if(pActionSO is SlamAttackSO slamSO)
        {
            return new SlamAttackAction(
                slamSO,
                _bossTransform,
                _playerTransform,
                _groundLayer,
                _impactZone
                );
        }

        if(pActionSO is CubeOrbSO orbSO)
        {
            return new CubeOrbAction(
                orbSO,
                _bossTransform,
                _playerTransform
                );
        }

        if(pActionSO is CubeDashSO cubeDashSO)
        {
            return new CubeDashAction(
                cubeDashSO,
                _bossTransform,
                _playerTransform
                );
        }

        Debug.LogError("Unknown BossActionSO type: " + pActionSO);
        return null;
    }
}
