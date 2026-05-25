using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;
using System;

public class BossActionController : MonoBehaviour
{
    [SerializeField] private EnemyHealth _bossHealth;
    
    [SerializeField] private List<BossActionSO> _actionSOList;
    private List<BossAction> _actions = new List<BossAction>();

    [SerializeField] private List<BossActionSO> _phase2ActionSOList;
    private List<BossAction> _phase2Actions = new List<BossAction>();
    private bool _phase2Pending = false;
    private bool _phase2Started = false;

    private int _currentActionIndex = 0;
    private int _previousActionIndex = -1;

    [SerializeField] private Transform _bossTransform;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private AudioClip _teleportSFX;
    [SerializeField] private AudioClip _orbShotSFX;
    [SerializeField] private AudioClip _audioCue;
    [SerializeField] private AudioClip _phase2AudioCue;
    [SerializeField] private AudioClip _impactSFX;
    [SerializeField] private CinemachineImpulseSource _groundImpulse;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private GameObject _impactZone;

    [SerializeField] private AudioClip _bossDefeatedSFX;
    [SerializeField] private AudioClip _victoryJingle;

    public event Action OnBossDefeated;

    private void Awake()
    {
        _bossTransform = transform;
        _bossHealth.OnEnemyDeath += BossDefeated;
    }

    private void Start()
    {

        foreach (var actionSO in _phase2ActionSOList)
        {
            BossAction action = CreateAction(actionSO);

            action.ActionFinished += OnActionFinished;
            _phase2Actions.Add(action);
        }

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

    private void OnDisable()
    {
        _bossHealth.OnEnemyDeath -= BossDefeated;
    }

    private void Update()
    {
        if (!_phase2Started && !_phase2Pending && _bossHealth.EnemyCurrentHealth <= _bossHealth.EnemyMaxHealth / 2)
        {
            _phase2Pending = true;
            //StartPhase2();
        }

        if (_actions.Count == 0)
            return;

        if (!_phase2Started)
            _actions[_currentActionIndex].UpdateAction();
        else
            _phase2Actions[_currentActionIndex].UpdateAction();
    }

    private void OnActionFinished()
    {
        if (_phase2Pending)
        {
            _phase2Pending = false;
            StartPhase2();
            return;
        }

        if (!_phase2Started)
        {
            _currentActionIndex++;

            if (_currentActionIndex >= _actions.Count)
                _currentActionIndex = 0;

            _actions[_currentActionIndex].StartAction();
        }
        else
        {
            _currentActionIndex = UnityEngine.Random.Range(0, _phase2Actions.Count);

            while (_currentActionIndex == _previousActionIndex)
            {
                _currentActionIndex = UnityEngine.Random.Range(0, _phase2Actions.Count);
            }

            _previousActionIndex = _currentActionIndex;

            _phase2Actions[_currentActionIndex].StartAction();
        }
    }

    private void StartPhase2()
    {
        Debug.Log("StartPhase2");
        _phase2Started = true;
        AudioManager.Instance.PlaySFX(_phase2AudioCue, false, 1f);

        _currentActionIndex = UnityEngine.Random.Range(0, _phase2Actions.Count);

        _phase2Actions[_currentActionIndex].StartAction();
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
                _impactZone,
                _impactSFX,
                _groundImpulse
                );
        }

        if(pActionSO is CubeOrbSO orbSO)
        {
            return new CubeOrbAction(
                orbSO,
                _bossTransform,
                _playerTransform,
                _orbShotSFX,
                _audioCue
                );
        }

        if(pActionSO is CubeDashSO cubeDashSO)
        {
            return new CubeDashAction(
                cubeDashSO,
                _bossTransform,
                _playerTransform,
                _teleportSFX,
                _impactSFX,
                _groundImpulse
                );
        }

        Debug.LogError("Unknown BossActionSO type: " + pActionSO);
        return null;
    }

    private void BossDefeated()
    {
        StartCoroutine(BossDefeatedCo());
    }

    private IEnumerator BossDefeatedCo()
    {
        yield return null;
        _playerTransform.GetComponent<Collider2D>().enabled = false;
        _playerTransform.GetComponent<Rigidbody2D>().simulated = false;
        UIManager.Instance.EnableFadePanel();
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.MusicSource.clip = null;
        AudioManager.Instance.PlaySFX(_bossDefeatedSFX);
        Time.timeScale = 0.15f;
        yield return new WaitWhile(() => AudioManager.Instance.SfxSource.isPlaying);
        //yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        AudioManager.Instance.StopLoopingSFX();
        AudioManager.Instance.PlaySFX(_victoryJingle);
        UIManager.Instance.EnableVictoryScreen();
    }
}
