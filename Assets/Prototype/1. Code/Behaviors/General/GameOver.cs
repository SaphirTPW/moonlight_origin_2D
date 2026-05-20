using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;
    //[SerializeField] private PlayerHealth _playerHealth;
    public void Retry()
    {
        if(GameManager.Instance.currentCheckpoint == null)
        {
            SceneManager.LoadScene(_sceneIndex);
            Time.timeScale = 1f;
            GameManager.Instance.UpdateGameState(GameManager.GameState.SetUp);
        }
        else
        {
            GameManager.Instance.UpdateGameState(GameManager.GameState.SetUp);
        }
    }
}
