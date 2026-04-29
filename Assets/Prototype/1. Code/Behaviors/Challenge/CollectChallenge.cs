using UnityEngine;

public class CollectChallenge : Challenge
{
    public CollectChallenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
       : base(pData, pRewardSpawn, pController) { }

    public void Collect(int amount)
    {
        if (!_isActive) return;

        AddProgress(amount);
    }
}
