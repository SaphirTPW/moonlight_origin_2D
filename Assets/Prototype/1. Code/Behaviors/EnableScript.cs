using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour
{
    [SerializeField] private ChallengeController challengeToEnable;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            challengeToEnable.gameObject.SetActive(true);
            StartCoroutine(StartNextFrame(challengeToEnable));
        }
    }

    private IEnumerator StartNextFrame(ChallengeController obj)
    {
        yield return null;

        var controller = obj.GetComponent<ChallengeController>();
        controller.Challenge.StartChallenge();
    }
}
