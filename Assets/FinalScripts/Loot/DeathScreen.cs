using UnityEngine;
using DefaultNamespace;
using UnityEngine.SceneManagement;

public class DeathScreen: MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Loading");
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnEnable()
    {
        MythUIElement.IsInUI = true;
    }

    public void OnDisable()
    {
        MythUIElement.IsInUI = false;
    }
}