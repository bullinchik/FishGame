
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int SceneName)
    {
        Debug.Log(SceneName);
        SceneManager.LoadScene(SceneName);
    }
}
