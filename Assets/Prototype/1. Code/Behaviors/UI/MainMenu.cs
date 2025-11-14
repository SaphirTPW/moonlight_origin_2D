using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadPlayRoom(string pSceneName)
    {
        SceneManager.LoadScene(pSceneName);
    }
}
