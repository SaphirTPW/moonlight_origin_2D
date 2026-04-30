using UnityEngine;

public class DestroyChallenge : Challenge
{
    public DestroyChallenge(ChallengeSO pData, Transform pRewardSpawn, ChallengeController pController)
       : base(pData, pRewardSpawn, pController) { }

    public void DestroyObject()
    {
        if (!IsActive) return;

        AddProgress(1);
    }
}
