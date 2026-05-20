using UnityEngine;
using System;

public abstract class Challenge
{
    private ChallengeSO _data;

    private int _currentAmount = 0;
    protected float _timer = 0f;

    protected bool _isCompleted = false;
    private bool _isActive = false;

    protected Transform _rewardSpawnPoint;
    protected ChallengeController _controller;

    public Action OnChallengeCompleted;
    public Action OnChallengeFailed;
    public Action OnChallengeStarted;

    public bool IsActive => _isActive;
    public bool IsCompleted => _isCompleted;
    public int CurrentAmount => _currentAmount;
    public ChallengeSO Data => _data;

    public Challenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
    {
        _data = pData;
        _rewardSpawnPoint = pRewardSpawn;
        _controller = pController;
    }

    public virtual void StartChallenge()
    {

        if (_isActive)
            return;

        if (_isCompleted)
            return;

        _isActive = true;
        _isCompleted = false;
        _currentAmount = 0;

        if (_data.useTimer)
            _timer = _data.timeLimit;

        OnChallengeStarted?.Invoke();
    }

    public virtual void UpdateChallenge()
    {
        if (!_isActive || _isCompleted)
            return;

        if (_data.useTimer)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
                FailChallenge();
        }
    }

    protected void AddProgress(int pAmount)
    {
        _currentAmount += pAmount;

        if (_currentAmount >= _data.targetAmount)
            CompleteChallenge();
    }

    protected virtual void CompleteChallenge()
    {
        _isCompleted = true;
        _isActive = false;

        OnChallengeCompleted?.Invoke();
    }

    protected virtual void FailChallenge()
    {
        _isActive = false;
        _timer = 0f;
        OnChallengeFailed?.Invoke();
    }
}
