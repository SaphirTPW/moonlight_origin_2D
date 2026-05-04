using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private int _sceneIndex;
    public void Retry()
    {
        if(GameManager.Instance.currentCheckpoint == null)
        {
            SceneManager.LoadScene(_sceneIndex);
            Time.timeScale = 1f;
        }
        else
        {
            GameManager.Instance.PlayerVoidOut();
            GameManager.Instance.UpdateGameState(GameManager.GameState.Playing);
        }
    }
}
