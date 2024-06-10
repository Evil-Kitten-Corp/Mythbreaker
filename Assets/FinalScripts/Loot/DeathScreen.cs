using UnityEngine;

public class DeathScreen: MonoBehaviour
{
    public void OnEnable()
    {
        MythUIElement.IsInUI = true; 
    }
    
    public void OnDisable()
    {
        MythUIElement.IsInUI = false;
    }

    public void Restart()
    {
        gameObject.SetActive(false);
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}