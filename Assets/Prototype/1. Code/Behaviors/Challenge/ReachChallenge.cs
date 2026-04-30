using UnityEngine;

public class ReachChallenge : Challenge
{
    public ReachChallenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
        : base(pData, pRewardSpawn, pController) { }

    public void ReachGoal()
    {
        if (!IsActive) return;

        AddProgress(1);
    }
}
