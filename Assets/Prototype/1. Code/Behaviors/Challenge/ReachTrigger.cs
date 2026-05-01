using UnityEngine;

public class ReachTrigger : MonoBehaviour
{
    [SerializeField] private ChallengeController _challenge;

    private void Start()
    {
        _challenge.Challenge.StartChallenge();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _challenge.ReachGoal();
        }
    }
}
