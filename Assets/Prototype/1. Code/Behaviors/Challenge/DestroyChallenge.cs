using UnityEngine;

public class DestroyChallenge : Challenge
{
    public DestroyChallenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
       : base(pData, pRewardSpawn, pController) { }

    public void DestroyObject()
    {
        if (!_isActive) return;

        AddProgress(1);
    }
}
