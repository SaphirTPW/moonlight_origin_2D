using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour
{
    [SerializeField] private ChallengeController challengeToEnable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            challengeToEnable.Challenge.StartChallenge();
        }
    }
}
