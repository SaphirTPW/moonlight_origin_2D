using UnityEngine;
using System.Collections;

public class EnableScript : MonoBehaviour
{
    [SerializeField] private GameObject objToEnable;

    private void Start()
    {
        objToEnable.GetComponent<ChallengeController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objToEnable.SetActive(true);
            StartCoroutine(StartNextFrame(objToEnable));
        }
    }

    private IEnumerator StartNextFrame(GameObject obj)
    {
        yield return null;

        var controller = obj.GetComponent<ChallengeController>();
        controller.Challenge.StartChallenge();
    }
}
