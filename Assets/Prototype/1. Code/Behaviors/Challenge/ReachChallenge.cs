using UnityEngine;

public class ReachChallenge : Challenge
{
    public ReachChallenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
        : base(pData, pRewardSpawn, pController) { }

    public void ReachGoal()
    {
        if (!_isActive) return;

        AddProgress(1);
    }
}
