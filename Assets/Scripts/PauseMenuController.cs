using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuPanel; // Reference to the PauseMenuPanel
    public GameObject controlsImage; // Reference to the ControlsImage

    private bool isPaused = false;

    void Update()
    {
        // Toggle pause menu when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        controlsImage.SetActive(false); // Hide the controls image if it's shown
        Time.timeScale = 1f; // Resume game time
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f; // Pause game time
        isPaused = true;
    }

    public void QuitGame()
    {
        // Add functionality to quit the game or load the main menu
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowControls()
    {
        // Show the controls image
        controlsImage.SetActive(true);
    }

    public void HideControls()
    {
        // Hide the controls image
        controlsImage.SetActive(false);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume game time before changing the scene
        SceneManager.LoadScene("MainMenu");
    }
}
