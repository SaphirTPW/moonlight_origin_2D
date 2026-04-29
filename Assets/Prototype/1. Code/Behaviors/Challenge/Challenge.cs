using UnityEngine;
using System;

public abstract class Challenge
{
    protected ChallengeSO _data;

    protected int _currentAmount = 0;
    protected float _timer = 0f;

    protected bool _isCompleted = false;
    protected bool _isActive = false;

    protected Transform _rewardSpawnPoint;
    protected ChallengeController _controller;

    public Action OnChallengeCompleted;
    public Action OnChallengeFailed;

    public Challenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
    {
        _data = pData;
        _rewardSpawnPoint = pRewardSpawn;
        _controller = pController;
    }

    public virtual void StartChallenge()
    {
        _isActive = true;
        _isCompleted = false;
        _currentAmount = 0;

        if (_data.useTimer)
            _timer = _data.timeLimit;
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
